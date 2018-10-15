using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CellEngine
{
    public class SaveData
    {
        private Header header;
        private Field loadedField;

        public void Load()
        {
            Engine.SwitchField(loadedField);
        }

        public void Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream);
            header = new Header(reader);
            Cell[,] cells = new Cell[header.SizeY, header.SizeX];

            for (int i = 0; i < header.SizeX * header.SizeY; i++)
            {
                int id = reader.ReadInt32();
                Header.CellInfo info = header.GetInfo(id);
                Cell cell = (Cell)Activator.CreateInstance(info.Type);

                byte[] buffer = new byte[info.DataLength]; //TODO make better solution for substream (maybe remove version-independent hack?)
                stream.Read(buffer, 0, buffer.Length);
                BinaryReader subReader = new BinaryReader(new MemoryStream(buffer));//To make save version-independend
                try
                {
                    cell.Deserialize(subReader);
                }
                catch (EndOfStreamException)
                {
                    Console.WriteLine("Save version mismatch at " + info.Type);
                }
                subReader.Close();
                cells[i / header.SizeY, i % header.SizeX] = cell;
            }
            loadedField = new Field(cells);
        }

        public static void Write(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream);
            Header.Write(writer);

            foreach (Cell cell in Engine.CurrentField.Cells())
            {
                Header.CellInfo info = Header.CurrentHeader.GetInfo(cell.GetType());
                writer.Write(info.Id);
                long position = stream.Position;
                cell.Serialize(writer);
                if (stream.Position != position + info.DataLength)
                    throw new EndOfStreamException("Somehow cell has managed to write more data than expected");
            }
        }

        public class Header
        {
            public static Header CurrentHeader;

            [StructLayout(LayoutKind.Sequential)]
            public struct CellInfo
            {
                public Type Type;
                public int Id;
                public int DataLength;
            }

            public List<CellInfo> Cells = new List<CellInfo>();
            public int SizeX;
            public int SizeY;

            static Header()
            {
                CurrentHeader = new Header(Engine.CurrentField.SizeX, Engine.CurrentField.SizeY);

                int i = 1;
                foreach (Type type in Assembly.GetEntryAssembly().GetTypes().Where(type => type.IsSubclassOf(typeof(Cell)) && !type.IsAbstract))
                {

                    CellInfo info = new CellInfo
                    {
                        Type = type,
                        Id = i,
                        DataLength = MeasureCellDataLength(type)
                    };
                    CurrentHeader.Cells.Add(info);
                    i++;
                }
            }

            private Header(int sizeX, int sizeY)
            {
                SizeX = sizeX;
                SizeY = sizeY;
            }

            public Header(BinaryReader reader) //TODO maybe move constructor to Read(BinaryReader reader)?
            {
                SizeX = reader.ReadInt32();
                SizeY = reader.ReadInt32();

                int length = reader.ReadInt32();

                for (int i = 0; i < length; i++)
                {
                    CellInfo info = new CellInfo
                    {
                        Type = Assembly.GetEntryAssembly().GetType(reader.ReadString(), true),
                        Id = reader.ReadInt32(),
                        DataLength = reader.ReadInt32()
                    };
                    Cells.Add(info);
                }
            }

            public static void Write(BinaryWriter writer)
            {
                writer.Write(CurrentHeader.SizeX);
                writer.Write(CurrentHeader.SizeY);

                writer.Write(CurrentHeader.Cells.Count);
                foreach (CellInfo info in CurrentHeader.Cells)
                {
                    writer.Write(info.Type.FullName);
                    writer.Write(info.Id);
                    writer.Write(info.DataLength);
                }
            }

            public static int MeasureCellDataLength(Type cellType) //Measure cell data length using dummy cell
            {
                Cell testCell = (Cell)Activator.CreateInstance(cellType); //TODO all cells has to have parameterless contructors. maybe workaround?
                Stream stream = new MemoryStream();
                testCell.Serialize(new BinaryWriter(stream));
                return (int)stream.Position;
            }

            public CellInfo GetInfo(Type cellType)
            {
                if (!cellType.IsSubclassOf(typeof(Cell)))
                    throw new ArgumentException("The type is not a cell type");

                return Cells.Find(cell => cell.Type == cellType);
            }

            public CellInfo GetInfo(int id)
            {
                for (int i = 0; i < Cells.Count; i++)
                    if (Cells[i].Id == id)
                        return Cells[i];
                throw new Exception("Unable to find cell with id " + id);
            }
        }
    }
}