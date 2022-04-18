using System;



namespace ClassLibrary1
{
    
	public class VMTime
	{
		public VMTime(VMGrid grid, VMF func, float time_ha, float time, float time_ep, float coef_ha, float coef_ep)
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
		public float VML_HA_Time { get; set; }
		public float VML_EP_Time { get; set; }
		public float Time_c { get; set; }

		//  время mkl / время без mkl
		public float VML_HA_Coef { get; set; }
		public float VML_EP_Coef { get; set; }

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
