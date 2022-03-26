using System;


namespace ClassLibrary1
{
	public class VMTime
	{
		public VMTime()
		{
			Params = new VMGrid();
			FunctionType = VMF.unmatched;
			VML_EP_Coef = 0;
			VML_HA_Coef = 0;
			VML_EP_Time = 0;
			VML_HA_Time = 0;
			Time_c = 0;
			more_info = "";
		}
		public VMTime(VMGrid grid, VMF func, double time_ha, double time, double time_ep, double coef_ha, double coef_ep)
		{
			Params = grid;
			FunctionType = func;
			VML_EP_Coef = coef_ep;
			VML_HA_Coef = coef_ha;
			VML_EP_Time = time_ep;
			VML_HA_Time = time_ha;
			Time_c = time;
			more_info = "";
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
		//Time_HA: {VML_HA_Time}\nTime_EP: {VML_EP_Time}\nTime_c: {Time_c}\n";
		public string StrView { get => ToString(); }
			
		
		//дополнительная информация
		private string more_info;
		public string MoreInfo
		{
			get => $"VML_HA coef: {VML_HA_Coef}\nVML_EP coef: {VML_EP_Coef}";
			set => more_info = value;
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
