/*
    Shellify, .NET implementation of Shell Link (.LNK) Binary File Format
    Copyright (C) 2010 Sebastien LEBRETON

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Shellify.ExtraData;
using System.Runtime.InteropServices;
using System.Text;
using System.Drawing;
using System;

namespace Shellify.IO
{
    public class ConsoleDataBlockHandler : ExtraDataBlockHandler<ConsoleDataBlock>
    {

        public const int UnusedLength = 8;
        public const int FaceNameLength = 64;
        public const int ColorTableLength = 64;

        public ConsoleDataBlockHandler(ConsoleDataBlock item, ShellLinkFile context)
            : base(item, context)
        {
        }

        public override int ComputedSize
        {
            get
            {
                return base.ComputedSize
                    + Marshal.SizeOf(typeof(short)) * 10
                    + UnusedLength 
                    + Marshal.SizeOf(typeof(int))*6
                    + Marshal.SizeOf(Item.FontWeight)
                    + FaceNameLength
                    + Marshal.SizeOf(Item.CursorSize)
                    + Marshal.SizeOf(Item.HistoryBufferSize)
                    + Marshal.SizeOf(Item.NumberOfHistoryBuffers)
                    + ColorTableLength ;
            }
        }

        public override void ReadFrom(System.IO.BinaryReader reader)
        {
            base.ReadFrom(reader);

            Item.FillAttributes = (FillAttributes)reader.ReadUInt16();
            Item.PopupFillAttributes = (FillAttributes)reader.ReadUInt16();

            Item.ScreenBufferSize = new Size(reader.ReadInt16(), reader.ReadInt16());
            Item.WindowSize = new Size(reader.ReadInt16(), reader.ReadInt16());
            Item.WindowOrigin = new Point(reader.ReadInt16(), reader.ReadInt16());

            reader.ReadBytes(UnusedLength);

            Item.FontSize = new Size(reader.ReadInt16(), reader.ReadInt16());
            Item.FontFamily = (Shellify.ExtraData.FontFamily)reader.ReadUInt32();
            Item.FontWeight = reader.ReadUInt32();
            
            Item.FaceName = IOHelper.ReadASCIIZF(reader, Encoding.Unicode, FaceNameLength);

            Item.CursorSize = reader.ReadUInt32();
            Item.FullScreen = reader.ReadUInt32() > 0;
            Item.FastEdit = reader.ReadUInt32() > 0;
            Item.InsertMode = reader.ReadUInt32() > 0;
            Item.AutoPosition = reader.ReadUInt32() > 0;

            Item.HistoryBufferSize = reader.ReadUInt32();
            Item.NumberOfHistoryBuffers = reader.ReadUInt32();
            Item.HistoryDuplicateAllowed = reader.ReadUInt32() > 0;
            Item.ColorTable = reader.ReadBytes(ColorTableLength);
        }

        public override void WriteTo(System.IO.BinaryWriter writer)
        {
            base.WriteTo(writer);
            writer.Write((ushort)Item.FillAttributes);
            writer.Write((ushort)Item.PopupFillAttributes);

            writer.Write((short)Item.ScreenBufferSize.Width);
            writer.Write((short)Item.ScreenBufferSize.Height);
            writer.Write((short)Item.WindowSize.Width);
            writer.Write((short)Item.WindowSize.Height);
            writer.Write((short)Item.WindowOrigin.X);
            writer.Write((short)Item.WindowOrigin.Y);

            writer.Write(new byte[UnusedLength]);

            writer.Write((short)Item.FontSize.Width);
            writer.Write((short)Item.FontSize.Height);
            writer.Write((uint)Item.FontFamily);
            writer.Write(Item.FontWeight);

            IOHelper.WriteASCIIZF(Item.FaceName, writer, Encoding.Unicode, FaceNameLength);

            writer.Write(Item.CursorSize);
            writer.Write(Convert.ToInt32(Item.FullScreen));
            writer.Write(Convert.ToInt32(Item.FastEdit));
            writer.Write(Convert.ToInt32(Item.InsertMode));
            writer.Write(Convert.ToInt32(Item.AutoPosition));

            writer.Write(Item.HistoryBufferSize);
            writer.Write(Item.NumberOfHistoryBuffers);
            writer.Write(Convert.ToInt32(Item.HistoryDuplicateAllowed));

            writer.Write(Item.ColorTable);
        }

    }
}
