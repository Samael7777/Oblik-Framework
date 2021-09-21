using System;


namespace Oblik.IO
{
    public partial class OblikConnector
    {
        
        /// <summary>
        /// Чтение данных из порта
        /// </summary>
        /// <param name="sp">Ссылка на порт</param>
        /// <param name="Timeout">Таймаут</param>
        /// <param name="BytesToRead">Количество байт для чтения</param>
        /// <param name="buffer">Буфер для считанных данных</param>
        private byte[] ReadAnswer(int BytesToRead, int timeout)
        {
            byte[] answer = new byte[BytesToRead];
            int BytesGet;
            int count = BytesToRead;
            int offset = 0;
            ulong start = GetTickCount();
            while (offset < BytesToRead)
            {
                if ((GetTickCount() - start) > (ulong)timeout)
                {
                    throw new Exception("Timeout");                             //TODO
                }
                try
                {
                    BytesGet = (byte)sp.Read(answer, offset, count);
                }
                catch
                {
                    BytesGet = 0;
                }
                count -= BytesGet;
                offset += BytesGet;
            }
            if (offset != BytesToRead) { throw new OblikIOException("ReadError"); }     //TODO
            return answer;
        }

        /// <summary>
        /// Отправка запроса к счетчику и получение данных
        /// </summary>
        /// <param name="Query">Запрос к счетчику в формате массива L1</param>
        /// <return>Ответ счетчика в формате массива L1</return>>
        public byte[] Request(byte[] Query)
        {
            bool success = false;           //Флаг успеха операции
            byte[] ReadBuffer;              //Буфер для полученных данных
            byte[] answer = new byte[3];
            try
            {
                try
                {
                    sp.Open();
                }
                catch (Exception e)
                {
                      throw new OblikIOException(e.Message);
                }
                
                int r = Repeats;

                while ((r > 0) && (!success))   //Повтор при ошибке
                {
                    sp.DiscardOutBuffer();                                                                 //очистка буфера передачи
                    sp.DiscardInBuffer();                                                                  //очистка буфера приема
                    try
                    {
                        if (Query == null)
                        {
                            throw new ArgumentNullException(paramName: nameof(Query));
                        }
                        sp.Write(Query, 0, Query.Length);
                    }
                    catch (Exception e)
                    {
                        throw new OblikIOException(e.Message);
                    }
                    try
                    {
                        answer = new byte[2];
                        r--;
                        //Получение результата L1
                        ReadBuffer = ReadAnswer(1, Timeout);
                        answer[0] = ReadBuffer[0];

                        //if (Answer[0] != 1) { throw new OblikIOException(ParseChannelError(Answer[0])); }

                        if (answer[0] != 1) 
                        { 
                            throw new OblikIOException("L1 error");             //TODO
                        }     
                        //Получение количества байт в ответе
                        ReadBuffer = ReadAnswer(1, Timeout);
                        answer[1] = ReadBuffer[0];
                        int len = ReadBuffer[0] + 1;
                        Array.Resize(ref answer, len + 2);
                        //Получение всего ответа
                        ReadBuffer = ReadAnswer(len, (int)(Timeout / 5u));
                        ReadBuffer.CopyTo(answer, 2);
                        success = (ReadBuffer.Length == len);

                        //if (answer[2] != 0) { throw new OblikIOException(ParseSegmentError(answer[2])); }
                        if (answer[2] != 0)
                        {
                            throw new OblikIOException("L2Error");              //TODO
                        }
                        //Проверка контрольной суммы
                        byte cs = 0;
                        for (int i = 0; i < answer.Length; i++)
                        {
                            cs ^= answer[i];
                        }
                        if (cs != 0)
                        {
                            throw new OblikIOException("CSCError");             //TODO
                        }
                    }
                    catch (Exception)
                    {
                        success = false;
                    }
                }
            }
            finally
            {
                if (sp != null) { sp.Dispose(); }
            }
            if (!success)
            {
                throw new OblikIOException("Query error");
            }
            return answer;
        }
    }
}
