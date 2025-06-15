using System;
public class program
{
	static bool Settup;
	static int Length;
	static int[] WarpedPosition(int[] Cell)
	{
		int[] WarpedPosition = new int[2];
		for (int i = 0; i < 2; i++)
			WarpedPosition[i] = (Cell[i] + Length) % Length;
		return WarpedPosition;
	}
	static int AliveNeighbourCount(int[] Cell,string[,] State, int Length, bool Wrap)
	{
		int AliveCount = 0;
		for (int Neighbour = 0 ; Neighbour < 9 ; Neighbour++)
		{	
			int[] NeighbourCell = {Cell[0]+(Neighbour/3)-1,Cell[1]+(Neighbour%3)-1};
			if (Wrap)
				NeighbourCell = WarpedPosition(NeighbourCell);
			if ( NeighbourCell[0]==Cell[0]&&NeighbourCell[1]==Cell[1])
				continue;
			if (IsCellAlive(NeighbourCell[0], NeighbourCell[1], State))
				AliveCount++;
		}
		return AliveCount;

	}

	static bool IsCellAlive(int Row ,int Column, string[,] State)
	{
		if (
			Row < 0
			|| Row > (State.GetLength(0) - 1)
			|| Column < 0
			|| Column > (State.GetLength(0) - 1)
		)
		{
			return false;
		}
		return State[Row, Column] == "\u25a0 " ;
	}

	static int[] AskSettings()
	{
		int[] Settings = new int[4];
		string[] PreFabSettings = {"(050 ms)Natural amount of sleep between generations:","(200)Natural amount of generations:", "(010)Natural size of grid:", "(001) Wrapped borders 1=On 0=Off:"};
		for (int i=0;i<4;i++)
		{
			int X;
			bool loop = true;
			while (loop == true)
			{
				Console.WriteLine(PreFabSettings[i]);
				string Answer = Console.ReadLine();
				if( Answer != "" && int.TryParse(Answer, out X))
				{
					Settings[i] = Convert.ToInt32(Answer);
					loop = false;
				}
				else if (Answer == "")
				{
					Settings[i] = Convert.ToInt32(PreFabSettings[i].Substring(1,3));
					loop = false;
				}
				else
				{
					Console.Clear();
				}
			}
		}
		Console.Clear();
		return Settings;
	}

	static string[,] InitialiseState(int Length, int Area)
	{
		string[,] State = new string[Length, Length];
		int i = 0;
		while(i < Area)
		{
			State[i / Length, i % Length] = "\u25a1 ";
			i++;
		}
		return State;
	}


	static void DrawState(int Length, int Area, string[,] State)
	{
		Console.Clear();
		int i = 0;
		while(i < Area)
		{	
			Console.Write(State[i / Length, i % Length]);
			if((i + 1) % Length == 0)Console.Write("\n");
			i++;
		}
	}

	static string[,] Construct(string[,] State, int[] Plan, int Cursor, int Length)
	{
		int CentralisingVector = (((int)Math.Sqrt(Plan.Length)) - 1) / 2;
		for (int i = 0; i < Plan.Length; i++)
		{
			if (Plan[i] == 1)
			{
				int[] Cell = {(Cursor/Length)+(i/(int)Math.Sqrt(Plan.Length))-CentralisingVector,(Cursor%Length)+(i%(int)Math.Sqrt(Plan.Length))-CentralisingVector};
				State[WarpedPosition(Cell)[0], WarpedPosition(Cell)[1]] = "\u25a0 ";
			}
		}
		return State;
	}

	static int ReadKeyPressed(int CursorPos, int Length, int Area, string[,] State)
	{
		var ch = Console.ReadKey(false).Key;
		switch(ch)
		{
				case ConsoleKey.RightArrow:
				Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
				Console.Write(State[CursorPos/Length,CursorPos%Length]);
				if(CursorPos == Area - 1)
					CursorPos = 0;
				else
					CursorPos = CursorPos + 1;
				break;
			case ConsoleKey.LeftArrow:
				Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
				Console.Write(State[CursorPos/Length,CursorPos%Length]);
				if(CursorPos == 0)
					CursorPos = Area - 1;
				else
					CursorPos = CursorPos - 1;
				break;
			case ConsoleKey.UpArrow:
				Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
				Console.Write(State[CursorPos/Length,CursorPos%Length]);
				if(CursorPos / Length == 0)
					CursorPos = CursorPos + Length * (Length - 1);
				else
					CursorPos = CursorPos - Length;
				break;
			case ConsoleKey.DownArrow:
				Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
				Console.Write(State[CursorPos/Length,CursorPos%Length]);
				if(CursorPos / Length == Length - 1)
					CursorPos = CursorPos - Length * (Length - 1);
				else
					CursorPos = CursorPos + Length;
				break;
			case ConsoleKey.X:
				if (State[CursorPos / Length, CursorPos % Length] == "\u25a1 ")
				{
					State[CursorPos / Length, CursorPos % Length] = "\u25a0 ";
					Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
					Console.Write("\u25a0 ");
				}
				else
				{
					State[CursorPos / Length, CursorPos % Length] = "\u25a1 ";
					Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
					Console.Write("\u25a1 ");
				}
				break;
			case ConsoleKey.Enter:
				Settup = false;
				break;
			case ConsoleKey.H:
				Console.Clear();
				string Help = "Constructs:\n	A: Acorn\n	G: Glider\n\n\nQ: Quit help window ";
				Console.Write(Help);
				while (Console.ReadKey(false).Key != ConsoleKey.Q)
				{
					Console.Clear();
					Console.Write(Help);
				}
				DrawState(Length, Area, State);
				break;
			case ConsoleKey.G:
				int[] PlanG = {
						1,0,0,
						0,1,1,
						1,1,0};
				State = Construct(State, PlanG, CursorPos, Length);
				DrawState(Length, Area, State);
				break;
			case ConsoleKey.A:
				int[] PlanA = {
						0,0,0,0,0,0,0,
						0,0,0,0,0,0,0,
						0,1,0,0,0,0,0,
						0,0,0,1,0,0,0,
						1,1,0,0,1,1,1,
						0,0,0,0,0,0,0,
						0,0,0,0,0,0,0};
				State = Construct(State, PlanA, CursorPos, Length);
				DrawState(Length, Area, State);
				break;
			default:
				break;
		}
		return CursorPos;
	}

	static string[,] GenerateNextState(int Length, int Area, string[,] State, bool Wrap)
	{
		string[,] NextState = new string[Length, Length];
		for(int i = 0; i < Area; i++)
		{
			int[] Cell = {i/Length,i%Length};
			int Pop = AliveNeighbourCount(Cell, State, Length, Wrap);

			if(State[Cell[0],Cell[1]] == "\u25a0 " && ( Pop < 2 || Pop > 3))
			{
				NextState[Cell[0],Cell[1]] = "\u25a1 ";
				Console.SetCursorPosition(2*Cell[1],Cell[0]);
				Console.Write("\u25a1 ");
				continue;
			}
			if(State[Cell[0], Cell[1]] == "\u25a1 " && Pop == 3)
			{
				NextState[Cell[0], Cell[1]] = "\u25a0 ";
				Console.SetCursorPosition(2*Cell[1],Cell[0]);
				Console.Write("\u25a0 ");
				continue;
			}
			else 
			{
				NextState[Cell[0], Cell[1]] = State[Cell[0], Cell[1]];
			}
		}
		return NextState;

	}
	public static void Main()
	{
		Console.Clear();
		Console.CursorVisible = false;
		
		int[] Settings = AskSettings();
		int Downtime = Settings[0]; 
		int Generationlim = Settings[1];
		Length = Settings[2];
		bool Wrap = Settings[3] == 1;

		int Area = Length * Length;
		string[,] State = InitialiseState(Length, Area);
		int CursorPos = 0;

		DrawState(Length, Area, State);
		
		Settup = true;
		while(Settup)
		{
			Console.SetCursorPosition(2*(CursorPos%Length),CursorPos/Length);
			Console.Write("X");
			CursorPos = ReadKeyPressed(CursorPos, Length, Area, State);
		}
		
		Console.Clear();		
		DrawState(Length, Area, State);

		for(int Generation = 0; Generation < (Generationlim + 1); Generation++)
		{
			string[,] NextState = GenerateNextState(Length, Area, State, Wrap);	
			System.Threading.Thread.Sleep(Downtime);
			State = NextState;
		}
		Console.CursorVisible = true;
		Console.Clear();
	}
}
