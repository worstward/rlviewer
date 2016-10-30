using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace RlViewer.Behaviors.Navigation.NavigationChanger.Abstract
{
    public abstract class NavigationChanger
    {
        public NavigationChanger(Files.LocatorFile fileToChange, Files.LocatorFile sourceFile)
        {
            DestinationFile = fileToChange;
            SourceFile = sourceFile;
        }


        protected Files.LocatorFile DestinationFile
        {
            get;
            private set;
        }

        protected Files.LocatorFile SourceFile
        {
            get;
            private set;
        }


        protected abstract int AzimuthCompressionCoef
        {
            get;
        }

        /// <summary>
        /// Changes selected rli flight time based on source rhg file
        /// </summary>
        public abstract void ChangeFlightTime();

        protected abstract float OffsetStrings
        {
            get;
        }



        /// <summary>
        /// Checks if RLI has .ba RHG as its source
        /// </summary>
        /// <param name="rliFile">Rli to check</param>
        /// <returns></returns>
        public static bool HasBaRhgSource(Files.LocatorFile rliFile)
        {
            if (rliFile == null)
            {
                return false;
            }

            return CheckBaExt(rliFile);
        }


        /// <summary>
        /// Checks if RLI has .ba RHG as its source
        /// </summary>
        /// <returns></returns>
        public bool HasBaRhgSource()
        {
            if (DestinationFile == null)
            {
                return false;
            }

            return CheckBaExt(DestinationFile);
        }


        private static bool CheckBaExt(Files.LocatorFile file)
        {
            var rliInfo = file.Header.HeaderInfo.Where(x => x.HeaderName == "РГГ")
                .FirstOrDefault();

            string rhgName = rliInfo.Params.Where(x => x.Item1 == "Имя файла").FirstOrDefault().Item2;

            return Path.GetExtension(rhgName) == ".ba";
        }



        /// <summary>
        /// Changes selected rli navigation based on source rhg file
        /// </summary>
        public void ChangeNavigation()
        {
            if (DestinationFile != null)
            {
                var strDataLength = SourceFile.Width * SourceFile.Header.BytesPerSample;

                using (var rhgStream = File.Open(SourceFile.Properties.FilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (var rlStream = File.Open(DestinationFile.Properties.FilePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        rlStream.Seek(DestinationFile.Header.FileHeaderLength, SeekOrigin.Begin);
                        rhgStream.Seek(SourceFile.Header.FileHeaderLength, SeekOrigin.Begin);
                        for (int i = 0; i < OffsetStrings; i++)
                        {
                            rhgStream.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * AzimuthCompressionCoef, SeekOrigin.Current);
                        }

                        for (int i = 0; i < DestinationFile.Height; i++)
                        {
                            var sourceStrHeader = GetSourceNavigationHeader(rhgStream);
                            var rlHeaderBytes = ConvertToDestStrHeaderBytes(sourceStrHeader);

                            rhgStream.Seek(strDataLength, SeekOrigin.Current);
                            rhgStream.Seek((SourceFile.Header.StrHeaderLength + strDataLength) * AzimuthCompressionCoef, SeekOrigin.Current);

                            rlStream.Write(rlHeaderBytes, 0, rlHeaderBytes.Length);
                            rlStream.Seek(DestinationFile.Width * DestinationFile.Header.BytesPerSample, SeekOrigin.Current);
                        }
                    }
                }


            }
        }




        protected abstract byte[] ConvertToDestStrHeaderBytes(Headers.Abstract.IStrHeader sourceHeader);

        protected Headers.Abstract.IStrHeader GetSourceNavigationHeader(System.IO.Stream s)
        {
            switch (SourceFile.Properties.Type)
            {
                case FileType.k:
                    return Behaviors.Converters.StructIO.ReadStruct<Headers.Concrete.K.KStrHeaderStruct>(s);
                case FileType.ba:
                    return Behaviors.Converters.StructIO.ReadStruct<Headers.Concrete.Ba.BaStrHeader>(s);
                default: 
                    throw new ArgumentException("SourceFile type");
            }             
        }


    }
}
