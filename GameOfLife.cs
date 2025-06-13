using System;
public class program
{
	static bool Settup;
	static int[] WarpedPosition(int Row, int Column, int Length)
	{
		int[] FinalPosition = new int[2];
		FinalPosition[0] = (Row + Length) % Length;
	        FinalPosition[1] = (Column + Length) % Length;
		return FinalPosition;
	}
	static int AliveNeighbourCount(int Row, int Column, string[,] State, bool Wrap)
	{
		int AliveCount = 0; 
		for (int Neighbour = 0 ; Neighbour < 9 ; Neighbour++ )
		{	
			int NeighbourRow = Row + ((Neighbour / 3) - 1);
			int NeighbourColumn = Column + ((Neighbour % 3) - 1);
			if (Wrap)
			{
				NeighbourRow = (NeighbourRow + State.GetLength(0)) % (State.GetLength(0));
				NeighbourColumn = (NeighbourColumn + State.GetLength(0)) % (State.GetLength(0));
			}

			if (NeighbourRow == Row && NeighbourColumn == Column)
			{
				continue;
			}

			if (IsCellAlive(NeighbourRow, NeighbourColumn, State))
			{
				AliveCount++;
			}
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
			Console.WriteLine(PreFabSettings[i]);
			string Answer = Console.ReadLine();
			if( Answer != "")
			{
				Settings[i] = Convert.ToInt32(Answer);
			}
			else
			{
				Settings[i] = Convert.ToInt32(PreFabSettings[i].Substring(1,3));
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
					State[WarpedPosition((Cursor / Length) + (i / (int)Math.Sqrt(Plan.Length)) - CentralisingVector, (Cursor % Length) + (i % (int)Math.Sqrt(Plan.Length)) - CentralisingVector, Length)[0], WarpedPosition((Cursor / Length) + (i / (int)Math.Sqrt(Plan.Length)) - CentralisingVector, (Cursor % Length) + (i % (int)Math.Sqrt(Plan.Length)) - CentralisingVector, Length)[1]] = "\u25a0 ";
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
			case ConsoleKey.G:
				int[] PlanG = {1,0,0,0,1,1,1,1,0};
				State = Construct(State, PlanG, CursorPos, Length);
				DrawState(Length, Area, State);
				break;
			case ConsoleKey.A:
				int[] PlanA = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
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
			int Row = i / Length;
			int Column = i % Length;
			int Pop = AliveNeighbourCount(Row, Column, State, Wrap);

			if(State[Row, Column] == "\u25a0 " && ( Pop < 2 || Pop > 3))
			{
				NextState[Row, Column] = "\u25a1 ";
				Console.SetCursorPosition(2*Column,Row);
				Console.Write("\u25a1 ");
				continue;
			}
			if(State[Row, Column] == "\u25a1 " && Pop == 3)
			{
				NextState[Row, Column] = "\u25a0 ";
				Console.SetCursorPosition(2*Column,Row);
				Console.Write("\u25a0 ");
				continue;
			}
			else 
			{
				NextState[Row, Column] = State[Row, Column];
			}
		}
		return NextState;

	}
	public static void Main()
	{
		int[] Settings = AskSettings();
		int Downtime = Settings[0]; 
		int Generationlim = Settings[1];
		int Length = Settings[2];
		bool Wrap = Settings[3] == 1;

		int Area = Length * Length;
		string[,] State = InitialiseState(Length, Area);
		int CursorPos = 0;

		Console.Clear();
		Console.CursorVisible = false;
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
