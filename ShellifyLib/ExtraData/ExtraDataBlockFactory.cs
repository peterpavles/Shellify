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

namespace Shellify.ExtraData
{
	public class ExtraDataBlockFactory
	{
		
		public static ExtraDataBlock GetInstance(ExtraDataBlockSignature signature)
		{
			switch (signature)
			{
                case ExtraDataBlockSignature.IconEnvironmentDataBlock:
                    return new IconEnvironmentDataBlock();
                case ExtraDataBlockSignature.EnvironmentVariableDataBlock:
                    return new EnvironmentVariableDataBlock();
                case ExtraDataBlockSignature.ConsoleDataBlock:
                    return new ConsoleDataBlock();
                case ExtraDataBlockSignature.PropertyStoreDataBlock:
                    return new PropertyStoreDataBlock();
                case ExtraDataBlockSignature.SpecialFolderDataBlock:
                    return new SpecialFolderDataBlock();
                case ExtraDataBlockSignature.KnownFolderDataBlock:
                    return new KnownFolderDataBlock();
                case ExtraDataBlockSignature.TrackerDataBlock:
                    return new TrackerDataBlock();
				default:
					return new UnknownDataBlock();
					
			}
		}		
	}
}
