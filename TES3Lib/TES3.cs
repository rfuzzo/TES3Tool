using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TES3Lib.Base;
using Utility;

namespace TES3Lib
{
    public class TES3
    {
        private const int HeaderSize = 16;
        public List<Record> Records { get; set; } = new();
        private Mutex RecordsMutex { get; } = new();
        public string Path { get; set; }

        public static TES3 TES3Load(string filePath, List<string> filteredGrops = null)
        {
            filteredGrops ??= new List<string>();

            var TES3 = new TES3() { Path = filePath };
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var header = new byte[HeaderSize];
                List<Task> tasks = [];
                while (fileStream.Read(header, 0, HeaderSize) != 0)
                {
                    fileStream.Position -= HeaderSize;

                    var reader = new ByteReader();
                    var name = reader.ReadBytes<string>(header, 4);
                    var size = reader.ReadBytes<int>(header);

                    if (!name.Equals("TES3") && filteredGrops.Count > 0 && !filteredGrops.Contains(name))
                    {
                        fileStream.Position += +HeaderSize + size;
                        continue;
                    }

                    var data = new byte[HeaderSize + size];
                    fileStream.Read(data, 0, HeaderSize + size);

                    TES3.AddRecordThreadSafe(null);
                    var index = TES3.Records.Count - 1;
                    tasks.Add(new Task(() => TES3.SetRecordAtIndexThreadSafe(index, BuildRecord(name, data))));
                    tasks[index].Start();
                }

                Task.WaitAll([.. tasks]);
            }

            return TES3;
        }

        public static TES3 TES3LoadSync(string filePath, List<string> filteredGrops = null)
        {
            filteredGrops ??= new List<string>();

            var TES3 = new TES3() { Path = filePath };
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                var header = new byte[HeaderSize];
                while (fileStream.Read(header, 0, HeaderSize) != 0)
                {
                    fileStream.Position -= HeaderSize;

                    var reader = new ByteReader();
                    var name = reader.ReadBytes<string>(header, 4);
                    var size = reader.ReadBytes<int>(header);

                    if (!name.Equals("TES3") && filteredGrops.Count > 0 && !filteredGrops.Contains(name))
                    {
                        fileStream.Position += +HeaderSize + size;
                        continue;
                    }

                    var data = new byte[HeaderSize + size];
                    fileStream.Read(data, 0, HeaderSize + size);

                    TES3.Records.Add(BuildRecord(name, data));
                }
            }

            return TES3;
        }

        public static Record BuildRecord(string name, byte[] data)
        {
            var assembly = Assembly.GetExecutingAssembly();
            return assembly
                .CreateInstance($"TES3Lib.Records.{name}", false, BindingFlags.Default, null, new object[] { data }, null, null) as Record;
        }

        public void TES3Save(string filePath)
        {
            using var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite);
            foreach (var record in Records)
            {
                var serializedRecord = record.SerializeRecord();

                fs.Write(serializedRecord, 0, serializedRecord.Length);
            }
        }

        /// <summary>
        /// Adds a record to the end of the Records list. Makes use of an internal mutex to ensure that it is thread-safe.
        /// </summary>
        /// <param name="record">The record to append.</param>
        public void AddRecordThreadSafe(Record record)
        {
            RecordsMutex.WaitOne();
            Records.Add(record);
            RecordsMutex.ReleaseMutex();
        }

        /// <summary>
        /// Sets a record at a given index in the Records list. Makes use of an internal mutex to ensure that it is thread-safe.
        /// </summary>
        /// <param name="index">The index to set at.</param>
        /// <param name="record">The record to set.</param>
        public void SetRecordAtIndexThreadSafe(int index, Record record)
        {
            RecordsMutex.WaitOne();
            Records[index] = record;
            RecordsMutex.ReleaseMutex();
        }
    }
}
