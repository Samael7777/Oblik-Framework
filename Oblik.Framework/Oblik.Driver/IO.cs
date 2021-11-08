/*
 * Базовый ввод-вывод
 */

using System;

namespace Oblik.Driver
{
    public partial class OblikDriver
    {

        /// <summary>
        /// Чтение данных из порта
        /// </summary>
        /// <param name="BytesToRead">Количество байт для чтения</param>
        /// <param name="index">Индекс начала записи в answer</param>
        /// <param name="answer">Массив с полученными данными</param>
        /// <returns></returns>
        private bool ReadAnswer(int BytesToRead, int index, ref byte[] answer)
        {

            //Таймаут на получение данных c ускорением для пакета
            int timeout = (BytesToRead > 1) ? (connectionParams.Timeout / 5) : connectionParams.Timeout;

            byte[] buffer = new byte[BytesToRead];      //Буфер для чтения

            int BytesGot;                               //Получено байт
            int count = BytesToRead;
            int offset = 0;
            ulong start = GetTickCount();
            while (offset < BytesToRead)
            {
                if ((GetTickCount() - start) > (ulong)timeout)
                {
                    //Таймаут
                    ErrorsLog.Add((int)Error.Timeout);
                    return false;
                }
                try
                {
                    BytesGot = (byte)sp.Read(buffer, offset, count);
                }
                catch
                {
                    BytesGot = 0;
                }
                count -= BytesGot;
                offset += BytesGot;
            }
            if (offset != BytesToRead)
            {
                //Ошибка чтения порта
                ErrorsLog.Add((int)Error.ReadError);
                return false;
            }
            //Сохранение полученных данных
            Array.Copy(buffer, 0, answer, index, BytesToRead);
            return true;
        }

        /// <summary>
        /// Отправка запроса L1 к счетчику и получение данных
        /// </summary>
        /// <param name="l1">Запрос L1 к счетчику</param>
        /// <returns>Ответ счетчика в формате массива L2</returns>
        public byte[] Request(byte[] l1)
        {
            //Исключение при пустом запросе
            if (l1 == null)
            {
                throw new ArgumentNullException(paramName: nameof(l1));
            }

            bool success = false;           //Флаг успеха операции
            byte[] answer = new byte[2];    //Буфер результата
            byte[] result = new byte[0];    //Буфер для ответа L2
            int L2len;                      //Количество байт в ответе L2

            //Очистка журнала ошибок
            ErrorsLog.Clear();

            //Попытка открытия порта
            try
            {
                sp.Open();
            }
            catch (Exception e)
            {
                ErrorsLog.Add((int)Error.OpenPortError);
                throw new OblikIOException(e.Message);
            }

            int r = connectionParams.Repeats + 1;

            while ((r > 0) && (!success))   //Повтор при ошибке
            {
                r--;
                //Очистка буферов
                sp.DiscardOutBuffer();
                sp.DiscardInBuffer();

                //Отправка запроса
                try
                {
                    sp.Write(l1, 0, l1.Length);
                }
                catch (Exception e)
                {
                    ErrorsLog.Add((int)Error.WriteError);
                    throw new OblikIOException(e.Message);
                }

                answer = new byte[2];

                //Получение результата L1
                if (!ReadAnswer(1, 0, ref answer))
                {
                    success = false;
                    continue;
                }

                //Проверка на ошибки L1
                if (answer[0] != 1)
                {
                    throw new OblikIOException(DecodeChannelError(answer[0]));
                }

                //Получение количества байт в ответе
                if (!ReadAnswer(1, 1, ref answer))
                {
                    success = false;
                    continue;
                }

                L2len = answer[1] + 1;
                Array.Resize(ref answer, L2len + 2);

                //Получение оставшихся данных
                if (!ReadAnswer(L2len, 2, ref answer))
                {
                    success = false;
                    continue;
                }

                //Проверка контрольной суммы ответа
                byte cs = 0;
                for (int i = 0; i < answer.Length; i++)
                {
                    cs ^= answer[i];
                }

                //Ошибка контрольной суммы ответа
                if (cs != 0)
                {
                    success = false;
                    ErrorsLog.Add((int)Error.CSCError);
                    continue;
                }
                
                success = true; //Запрос выполнен успешно
                
                //Формирование ответа L2
                if (L2len > 0)
                {
                    Array.Resize(ref result, L2len);
                    Array.Copy(answer, 2, result,0, L2len);
                }
            }
            //Закрытие порта
            if ((sp != null) && (sp.IsOpen))
            {
                sp.Close();
            }
            //Исключение при неудачном приеме
            if (!success)
            {
                 throw new OblikIOException("IO request execution error");
            }

            return result;
        }

        /// <summary>
        /// Возвращает количество миллисекунд для данного экземпляра
        /// </summary>
        /// <returns>Количество миллисекунд для данного экземпляра</returns>
        private static ulong GetTickCount()
        {
            return (ulong)DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        /// <summary>
        /// Парсер ошибок L1
        /// </summary>
        /// <param name="error">Код ошибки L1</param>
        /// <returns>Строка с текстом ошибки</returns>
        private string DecodeChannelError(int error)
        {
            string res;
            switch (error)
            {
                case 1:
                    res = "L1 OK";
                    break;
                case 0xff:
                    res = "L1 Checksum Error";
                    break;
                case 0xfe:
                    res = "L1 Meter input buffer overflow";
                    break;
                default:
                    res = "L1 Unknown error";
                    break;
            }
            return res;
        }
    }
}
