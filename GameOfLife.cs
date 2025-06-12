using System;
public class program
{
	static bool Wrap;
	static bool Settup;

	static int[] WarpedPosition(int Row, int Column, int Length)
	{
		int[] FinalPosition = new int[2];
		FinalPosition[0] = (Row + Length) % Length;
	        FinalPosition[1] = (Column + Length) % Length;
		return FinalPosition;
	}
	static int AliveNeighbourCount(int Row, int Column, string[,] State)
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
		int[] Settings = new int[3];
		string[] PreFabSettings = {"(050 ms)Natural amount of sleep between generations:","(200)Natural amount of generations:", "(010)Natural size of grid:"};
		for (int i=0;i<3;i++)
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
		return Settings;
	}

	static bool AskWrap()
	{
		Console.Write("wrapped border or dead border (W/d):");
		if(Console.ReadLine() == "d")
		{
			return false;
		}
		Console.Clear();
		return true;
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
		int i = 0;
		while(i < Area)
		{	
			Console.Write(State[i / Length, i % Length]);
			if((i + 1) % Length == 0)Console.Write("\n");
			i++;
		}
	}

	static string[,] Construct(string[,] State, int[] Plan, int Cursor, int Length, string Hold)
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

	static int ReadKeyPressed(int Cursor, int Length, int Area, string Hold, string[,] State)
	{
		State[Cursor / Length, Cursor % Length] = Hold;
		var ch = Console.ReadKey(false).Key;
		switch(ch)
		{
			case ConsoleKey.RightArrow:
				if(Cursor == Area - 1)
					Cursor = 0;
				else
					Cursor = Cursor + 1;
				break;
			case ConsoleKey.LeftArrow:
				if(Cursor == 0)
					Cursor = Area - 1;
				else
					Cursor = Cursor - 1;
				break;
			case ConsoleKey.UpArrow:
				if(Cursor / Length == 0)
					Cursor = Cursor + Length * (Length - 1);
				else
					Cursor = Cursor - Length;
				break;
			case ConsoleKey.DownArrow:
				if(Cursor / Length == Length - 1)
					Cursor = Cursor - Length * (Length - 1);
				else
					Cursor = Cursor + Length;
				break;
			case ConsoleKey.X:
				if (Hold == "\u25a1 ")
					State[Cursor / Length, Cursor % Length] = "\u25a0 ";
				else
					State[Cursor / Length, Cursor % Length] = "\u25a1 ";
				break;
			case ConsoleKey.Enter:
				Settup = false;
				break;
			case ConsoleKey.G:
				int[] PlanG = {1,0,0,0,1,1,1,1,0};
				State = Construct(State, PlanG, Cursor, Length, Hold);
				break;
			case ConsoleKey.A:
				int[] PlanA = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,1,0,0,0,1,1,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
				State = Construct(State, PlanA, Cursor, Length, Hold);
				break;
			default:
				break;
		}
		Console.Clear();
		return Cursor;
	}

	static string[,] GenerateNextState(int Length, int Area, string[,] State)
	{
		string[,] NextState = new string[Length, Length];
		for(int i = 0; i < Area; i++)
		{
			int Row = i / Length;
			int Column = i % Length;
			int Pop = AliveNeighbourCount(Row, Column, State);

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
		Console.Clear();
		
		int[] Settings = AskSettings();
		int Downtime = Settings[0]; 
		int Generationlim = Settings[1];
		int Length = Settings[2];

		int Area = Length * Length;
		
		Wrap = AskWrap();
		
		string[,] State = InitialiseState(Length, Area);
		
		int Cursor = 0;
		Settup = true;
		while(Settup)
		{
			string Hold = State[Cursor / Length, Cursor % Length];
			State[Cursor / Length, Cursor % Length] = "X ";

			DrawState(Length, Area, State);
			Console.WriteLine("(" + Hold + ")\nToggle (x)\nExecute(Rtn)");

			Cursor = ReadKeyPressed(Cursor, Length, Area, Hold, State);
		}
				
		DrawState(Length, Area, State);

		for(int Generation = 0; Generation < (Generationlim + 1); Generation++)
		{
			string[,] NextState = GenerateNextState(Length, Area, State);	
			System.Threading.Thread.Sleep(Downtime);
			State = NextState;
		}

		Console.Clear();
	}
}
