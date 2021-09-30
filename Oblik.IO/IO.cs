/*
 * Базовый ввод-вывод
 */

using System;

namespace Oblik.IO
{
    public partial class OblikConnector
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
            int to = (BytesToRead > 1) ? (Timeout / 5) : Timeout;

            byte[] buffer = new byte[BytesToRead];      //Буфер для чтения

            int BytesGot;                               //Получено байт
            int count = BytesToRead;
            int offset = 0;
            ulong start = GetTickCount();
            while (offset < BytesToRead)
            {
                if ((GetTickCount() - start) > (ulong)to)
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
        /// <return>Ответ счетчика в формате массива L1</return>>
        public byte[] Request()
        {
            //Исключение при пустом запросе
            if (l1 == null)
            {
                throw new ArgumentNullException(paramName: nameof(l1));
            }

            bool success = false;           //Флаг успеха операции
            byte[] answer = new byte[2];    //Буфер результата

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

            int r = repeats + 1;

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

                int len = answer[1] + 1;
                Array.Resize(ref answer, len + 2);

                //Получение оставшихся данных
                if (!ReadAnswer(len, 2, ref answer))
                {
                    success = false;
                    continue;
                }

                //Проверка на ошибки L2
                if (answer[2] != 0)
                {
                    throw new OblikIOException(DecodeSegmentError(answer[2]));
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
                success = true;
            }
            //Закрытие порта
            if ((sp != null) && (sp.IsOpen))
            {
                sp.Close();
            }
            //Исключение при неудачном приеме
            if (!success)
            {
                ErrorsLog.Add((int)Error.QueryError);
                throw new OblikIOException(Resources.Resources.QueryErr);
            }
            return answer;
        }
    }
}
