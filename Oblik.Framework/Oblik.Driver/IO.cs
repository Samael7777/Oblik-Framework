/*
 * Базовый ввод-вывод
 */

using System;

namespace Oblik.Driver
{
    public partial class OblikSerialDriver
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
            int timeout = connectionParams.Timeout;
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
                    sp.Close();
                    throw new OblikIOException("Timeout", Error.Timeout);                    
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
                sp.Close();
                throw new OblikIOException("Port read error", Error.ReadError);
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

            byte[] answer = new byte[2];    //Буфер результата
            byte[] result = new byte[0];    //Буфер для ответа L2
            int L2len;                      //Количество байт в ответе L2

            //Попытка открытия порта
            try
            {
                if (sp.IsOpen)
                {
                    sp.Close();
                }
                sp.Open();
            }
            catch (Exception e)
            {
                throw new OblikIOException(e.Message, Error.OpenPortError);
            }

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
                sp.Close();
                throw new OblikIOException(e.Message, Error.WriteError);
            }

            //Получение результата L1
            ReadAnswer(1, 0, ref answer);
            //Проверка на ошибки L1
            if (answer[0] != 1)
            {
                sp.Close(); //Закрытие порта
                DecodeChannelError(answer[0]);
            }

            //Получение количества байт в ответе
            ReadAnswer(1, 1, ref answer);
            L2len = answer[1] + 1;
            Array.Resize(ref answer, L2len + 2);

            //Получение оставшихся данных
            ReadAnswer(L2len, 2, ref answer);
            
            sp.Close(); //Закрытие порта

            //Проверка контрольной суммы ответа
            byte cs = 0;
            for (int i = 0; i < answer.Length; i++)
            {
                cs ^= answer[i];
            }

            //Ошибка контрольной суммы ответа
            if (cs != 0)
            {
                throw new OblikIOException("Answer L1 CSC Error", Error.L1CSCError);
            }

            //Формирование ответа L2
            if (L2len > 0)
            {
                Array.Resize(ref result, L2len);
                Array.Copy(answer, 2, result, 0, L2len);
            }
            //Закрытие порта
            if ((sp != null) && (sp.IsOpen))
            {
                sp.Close();
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
        /// Парсер ошибок L1, при ошибке вызывает исключение OblikIOException
        /// </summary>
        /// <param name="error">Код ошибки L1</param>
        private void DecodeChannelError(int error)
        {
            switch (error)
            {
                case 1:
                    break;  //No errors
                case 0xff:
                    throw new OblikIOException("L1 Checksum Error", Error.L1CSCError);
                case 0xfe:
                    throw new OblikIOException("L1 Meter input buffer overflow", Error.L1BufOverfowError);
                default:
                    throw new OblikIOException("L1 Unknown error", Error.L1UnkErrror);                 
            }
        }
    }
}
