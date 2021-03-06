﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RlViewer.Files;
using RlViewer.Factories.File.Concrete;

namespace RlViewer.Factories.File.Abstract
{
    public abstract class FileFactory
    {

        public abstract LocatorFile Create(FileProperties properties, Headers.Abstract.LocatorFileHeader header, RlViewer.Navigation.NavigationContainer navi);

        public static FileFactory GetFactory(FileProperties properties)
        {
            switch (properties.Type)
            {
                case FileType.brl4:
                    return new Brl4Factory();
                case FileType.rl4:
                    return new Rl4Factory();
                case FileType.raw:
                    return new RawFactory();
                case FileType.r:
                    return new RFactory();
                case FileType.k:
                    return new KFactory();
                case FileType.rl8:
                    return new Rl8Factory();
                case FileType.ba:
                    return new BaFactory();
                default:
                    throw new NotSupportedException("Unsupported file format");
            }
        }

    }
}
