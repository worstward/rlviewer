using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace RlViewer.Behaviors.Synthesis
{

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct ServerSarTaskParams
    {
        public int struct_id;                               // идентификатор структуры

        // файлы и директории

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] input_dir;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] input_file;                        // путь к файлу РГГ (*.gl1, *.gl2, *.gl4, *.gl8, *.gol)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] input_file_K1;                        // путь к файлу РГГ (*.gl1, *.gl2, *.gl4, *.gl8, *.gol)
        [MarshalAs(UnmanagedType.I1)]
        public bool output;                                 // запись файлов вычислений
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] output_dir;                        // директория файлов вычислений(server)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] output_dir_loc;                    // директория файлов вычислений(client)
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] output_file;                       // директория файлов вычислений
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
        public byte[] Client_ip_address;

        public int RGG_RLI_DSP_numbers;					 // количество массивов в общей памяти для РГГ, РЛИ, ДСП

        // диапазон синтеза
        public int M;										 // номер текущего блока по азимуту
        public int ny_start;                               // начальный отсчет по азимуту
        public int ny_start_initial;
        public int Mshift;                                 // размер кадра по азимуту
        public int Mlength;                                // размер блока по азимуту
        public int Mparts;                                 // число блоков по азимуту
        public float Mscale;                                 // коэффициент сжатия по азимуту
        public float Mscale_initial;                         // коэффициент сжатия по азимуту

        public int N;										 // номер текущего блока по дальности
        public int nx_start;                               // начальный отсчет по дальности
        public int nx_start_initial;
        public int Nshift;                                 // размер кадра по дальности
        public int Nlength;                                // размер блока по дальности
        public int Nstripes;                               // число блоков по дальности
        public float Nscale;                                 // коэффициент сжатия по дальности
        public float Nscale_initial;                         // коэффициент сжатия по дальности

        // параметры ЛЧМ
        public float lambda;                                // длина волны (м)
        public float f0;                                    // центральная частота ЛЧМ (Гц)
        public float Tp;                                    // длительность ЛЧМ импульса (мс)
        public float dR;                                    // дискрет по дальности (м)

        // геометрия и параметры полета
        public float Vfly;                                  // скорость полета (м/с)
        public float Fimpulses;                             // частота повторения импульсов (Гц)
        public float Rmin;                                  // минимальная дальность (м)
        public float Hc;                                    // высота (м)
        public float Angle;                                 // угол раскрыва апертуры (градус)
        public int Azimut_chirp_sign;                     // знак опорной функции по азимуту
        public int Range_chirp_sign;                      // знак опорной функции по дальности

        // навигация
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] Nav_File;                         // путь к файлу навигации (*.csv)
        [MarshalAs(UnmanagedType.I1)]
        public bool Moco1;                                 // компенсация ТН
        [MarshalAs(UnmanagedType.I1)]
        public bool interpolate1;                          // смещение по дальности
        public int nLobs_i1;                                 // число отсчетов Sinc
        [MarshalAs(UnmanagedType.I1)]
        public bool Moco1_Nav_Err;

        [MarshalAs(UnmanagedType.I1)]
        public bool Moco1_Range_Dependent;                 // компенсация ТН по дальности
        [MarshalAs(UnmanagedType.I1)]
        public bool Moco2;
        [MarshalAs(UnmanagedType.I1)]
        public bool Moco2_Range_Dep;

        public int Nav_LR_Side;                           // борт наблюдения (правый +1, левый -1)
        public int Nav_Start;                             // стартовая метка в файле навигации (с)
        public float Hol_Start;                             // стартовая метка в файле голограммы (с)
        public int Hol_Y_Beg;
        public int Hol_Y_End;
        public int Cheb_approx_order;                     // порядок многочленов Чебышева

        // параметры синтеза
        [MarshalAs(UnmanagedType.I1)]
        public bool Do_range_compress;                     // сжатие по дальности
        [MarshalAs(UnmanagedType.I1)]
        public bool interpolate;                           // интерполяция
        public int nLobs;                                 // число отсчетов Sinc

        public int Algorithm_choice;                      // алгоритм синтеза (0-Омега-К, 1-Интерполяция Столта, 2-ПФА, 3-Быстрая свертка, 4-ЛЧМ-масштабирование, 5-Согласованная фильтрация)
        public float Doppler_Max;                           // максимальная доплеровская частота
        public float Doppler_Min;                           // минимальная доплеровская частота
        [MarshalAs(UnmanagedType.I1)]
        public bool EOK_With_CZT;                          // ЛЧМ-фильтрация
        [MarshalAs(UnmanagedType.I1)]
        public bool Range_kx_compression;
        /////////////////// new part ////////////////////////////	

        public int cntPoints;								// количество точек автофокуса
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] ppx;								// координаты точек автофокуса
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public int[] ppy;

        public int dsp_zone_width;
        public int frame_width;
        public int n_of_frames;
        public int frame_number;
        public int AzimComp_width;

        [MarshalAs(UnmanagedType.I1)]
        public bool cutAngle;
        [MarshalAs(UnmanagedType.I1)]
        public bool cutDoppler;

        public int pNstripes;
        public int pNlength;
        public int pNshift;

        [MarshalAs(UnmanagedType.I1)]
        public bool Nonius;
        public float Times_mc;
        [MarshalAs(UnmanagedType.I1)]
        public bool Eok_RC_with_F2d;
        [MarshalAs(UnmanagedType.I1)]
        public bool permut_x;
        [MarshalAs(UnmanagedType.I1)]
        public bool Ws_wFs;
        [MarshalAs(UnmanagedType.I1)]
        public bool FormatRli;
        /// Simul ///

        [MarshalAs(UnmanagedType.I1)]
        public bool LoopBlocking;
        [MarshalAs(UnmanagedType.I1)]
        public bool simul_Y_N;
        public int simul_X;
        public int simul_Y;
        public int ntarget;
        [MarshalAs(UnmanagedType.I1)]
        public bool simul_motion;
        public float motion_A2;
        public float motion_B2;
        public float motion_A3;
        public float motion_B3;
        [MarshalAs(UnmanagedType.I1)]
        public bool simul_clear_input;
        public float simul_mult;
        ////////////////////////////////
        public int simul_pattern;
        public int simul_N_cnt;
        public int simul_M_cnt;
        //AF2
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] AF_FileName;

        [MarshalAs(UnmanagedType.I1)]
        public bool AF_Do_Y_N;
        [MarshalAs(UnmanagedType.I1)]
        public bool AF_FromFile_Y_N;
        [MarshalAs(UnmanagedType.I1)]
        public bool AF_FromFileNameList_Y_N;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] AF_FromFileName;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] AF_FromFileNameList;

        public int AF_ncycle;
        public int AF_WinX;
        public int AF_WinY;

        public int AF_pX;
        public int AF_pY;

        ///////////
        [MarshalAs(UnmanagedType.I1)]
        public bool AF2_Y_N;
        public int AF2_ncycle;
        public float AF2_percent;
        public int lc_x;
        public int lc_y;
        [MarshalAs(UnmanagedType.I1)]
        public bool TRAJ_DUMP;
        [MarshalAs(UnmanagedType.I1)]
        public bool F2d_Inmem;
        [MarshalAs(UnmanagedType.I1)]
        public bool F1d_Inmem;
        [MarshalAs(UnmanagedType.I1)]
        public bool Vs_Inmem;
        //
        public int Memory_parts;
        [MarshalAs(UnmanagedType.I1)]
        public bool FromHol_Y_N;
        [MarshalAs(UnmanagedType.I1)]
        public bool clearAll;
        [MarshalAs(UnmanagedType.I1)]
        public bool Fcs_Inmem;

        [MarshalAs(UnmanagedType.I1)]
        public bool simul_in_file_Y_N;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] simul_file;
        public int simul_write_format;
        public float simul_dna_angle;

        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep0;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep1;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep2;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep3;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep4;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep5;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep6;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep7;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep8;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep9;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep10;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep11;
        [MarshalAs(UnmanagedType.I1)]
        public bool DumpStep12;

        public float Vxs, Vys, Vhs;
        public float Ax, Bx, Ay, By, Ah, Bh;
        public float Vxerr, Vyerr, Vherr;
        public float Nfreq;
        [MarshalAs(UnmanagedType.I1)]
        public bool simul_vel_approx;

        [MarshalAs(UnmanagedType.I1)]
        public bool Vel_approx;
        public uint simul_SEED;

        /////////// FBP family ///////////////////
        [MarshalAs(UnmanagedType.I1)]
        public bool fbp_moco;
        public int fbp_algorithm;
        public int fbp_RnUp;
        public int fbp_AnUp;
        public int fbp_NAP;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public byte[] fname_K;
        [MarshalAs(UnmanagedType.I1)]
        public bool Do_Convert_K;
        public int Pga_af_shift;
        public float AlphaCosPedestal;
        public int AF_Alg;
        public int XNSum1024YNSum;
        public int DoFastConvOnFullFrame;
        public int MAMD_NIter;
        public float RadioSuppression;
        [MarshalAs(UnmanagedType.I1)]
        public bool fbp_cuda;
        public int fbp_sign;
        public int fbp_len;

        public float fbp_durli;
        public int fbp_Nsub;
        public int fbp_Nstripes;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
        public byte[] fbp_KoeffSumming;
        public float fbp_drrli;
        //public int pDoc;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 936)]
        public byte[] reserved; //[];

        // Параметры обмена
        public int sarServerCmd;		// команда: 1 - применить новые параметры, 0x1124 - завершить работу
        public int sarServerRet;		// ответ:   0x100 - параметры установлены, 0x1125 - работа завершена
    };
}
