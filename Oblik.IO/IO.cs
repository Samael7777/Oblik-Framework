/*
 * Базовый ввод-вывод
 */

using System;
using Oblik.Resources;

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
            int to = (BytesToRead > 1)? (Timeout / 5) : Timeout;  
            
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
                    LastReceiverError = Resources.Resources.Timeout;
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
                LastReceiverError = Resources.Resources.ReadError;
                return false;
            }
            //Сохранение полученных данных
            Array.Copy(buffer, 0, answer, index, BytesToRead);
            return true;
        }

        /// <summary>
        /// Отправка запроса к счетчику и получение данных
        /// </summary>
        /// <param name="Query">Запрос к счетчику в формате массива L1</param>
        /// <return>Ответ счетчика в формате массива L1</return>>
        public byte[] Request(byte[] Query)
        {
            //Исключение при пустом запросе
            if (Query == null)
            {
                throw new ArgumentNullException(paramName: nameof(Query));
            }
            
            bool success = false;           //Флаг успеха операции
            byte[] answer = new byte[2];    //Буфер результата
            
            //Попытка открытия порта
            try
            {
                sp.Open();
            }
            catch (Exception e)
            {
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
                    sp.Write(Query, 0, Query.Length);
                }
                catch (Exception e)
                {
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
                    throw new OblikIOException(ParseChannelError(answer[0])); 
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
                    throw new OblikIOException(ParseSegmentError(answer[2])); 
                }
                       
                //Проверка контрольной суммы
                byte cs = 0;
                for (int i = 0; i < answer.Length; i++)
                {
                    cs ^= answer[i];
                }
                        
                //Ошибка контрольной суммы
                if (cs != 0)
                {
                    success = false;
                    LastReceiverError = Resources.Resources.CSCError;            
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
                throw new OblikIOException(LastReceiverError);
            }
            return answer;
        }
    }
}
