using System;



namespace ClassLibrary1
{
    
	public class VMTime
	{
		public VMTime(VMGrid grid, VMF func, double time_ha, double time, double time_ep, double coef_ha, double coef_ep)
		{
			Params = grid;
			FunctionType = func;
			VML_EP_Coef = coef_ep;
			VML_HA_Coef = coef_ha;
			VML_EP_Time = time_ep;
			VML_HA_Time = time_ha;
			Time_c = time;
		}
		//свойство для параметров сетки
		public VMGrid Params { get; set; }

		//свойтсво с информацией о том, для какой функции выполнялось тестирование
		public VMF FunctionType { get; set; }

		//свойства для времени вычисления
		public double VML_HA_Time { get; set; }
		public double VML_EP_Time { get; set; }
		public double Time_c { get; set; }

		//  время mkl / время без mkl
		public double VML_HA_Coef { get; set; }
		public double VML_EP_Coef { get; set; }

		public override string ToString() => Params.ToString() + $"name func: {NameFunc}";


        //дополнительная информация
        public string MoreInfo
        {
        	get => $"VML_HA coef: {VML_HA_Coef}\nVML_EP coef: {VML_EP_Coef}";
        }

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
