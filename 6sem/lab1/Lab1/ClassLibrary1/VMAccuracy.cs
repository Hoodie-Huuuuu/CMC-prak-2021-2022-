using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary1
{
    public class VMAccuracy
    {
        public VMAccuracy()
        {
            Params = new VMGrid();
            FunctionType = VMF.unmatched;
            MaxDif = 0;
            MaxDifArg = 0;
            MaxDifValue_VML_HA = 0;
            MaxDifValue_VML_EP = 0;
            more_info = "";

        }
        public VMAccuracy(VMGrid grid, VMF func, double max_dif, double max_dif_arg, double ha, double ep)
        {
            Params = grid;
            FunctionType = func;
            MaxDif = max_dif;
            MaxDifArg = max_dif_arg;
            MaxDifValue_VML_HA = ha;
            MaxDifValue_VML_EP = ep;
            more_info = "";
        }
        //параметры сетки
        public VMGrid Params { get; set; }

        //информация о том, для какой функции выполнялось тестирование
        public VMF FunctionType { get; set; }

        //максимальное значение модуля разности значений, вычисленных
        //в режимах VML_HA и WML_EP;
        public double MaxDif { get; set; }

        //свойство для значения аргумента функции, при котором максимально отличаются
        //значения функции, вычисленные в режимах VML_HA WML_EP
        public double MaxDifArg { get; set; }

        //соответствующие значения функции при аргументе MaxDifArg
        public double MaxDifValue_VML_HA { get; set; }

        public double MaxDifValue_VML_EP { get; set; }

        public override string ToString() => Params.ToString() + $"name func: {NameFunc}";
        public string StrView { get => ToString(); }

        //дополнительная информация
        private string more_info;
        public string MoreInfo
        {
            get => $"MaxDifArg: {MaxDifArg}\nValue VML_HA: {MaxDifValue_VML_HA}\nValue VML_EP: {MaxDifValue_VML_EP}";
            set => more_info = value;
        }

        //возвращает имя функции
        public string NameFunc
        {
            get
            {
                switch (FunctionType)
                {
                    case VMF.vmsExp:
                        return "Exp float";
                    case VMF.vmdExp:
                        return "Exp double";
                    case VMF.vmsErf:
                        return "Erf float";
                    case VMF.vmdErf:
                        return "Erf double";
                }
                return "unmachted";
            }

        }
    }
}