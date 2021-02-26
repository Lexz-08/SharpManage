using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace SharpManage
{
	public struct Functions
	{
		internal static Process[] GetSpecificProc_List(string ProcessName)
		{
			Process[] detectedProcesses = Process.GetProcessesByName(ProcessName);
			if (detectedProcesses.Length > 0) return detectedProcesses;
			else throw new Exception("No processes of name '" + ProcessName + "' were found.");
		}
		internal static Process GetSpecificProc(string ProcessName)
		{
			try
			{
				return GetSpecificProc_List(ProcessName)[0];
			}
			catch
			{
				throw new Exception("No process of name '" + ProcessName + "' were found.");
			}
		}
		internal static void KillProc_List(string ProcessName)
		{
			try
			{
				Process[] detectedProcesses = GetSpecificProc_List(ProcessName);
				foreach (Process detectedProcess in detectedProcesses)
				{
					detectedProcess.Kill();
				}
			}
			catch
			{
				throw new Exception("No processes of name '" + ProcessName + "' were killed.");
			}
		}
		internal static void KillProc(string ProcessName)
		{
			try
			{
				GetSpecificProc(ProcessName).Kill();
			}
			catch
			{
				throw new Exception("No process of name '" + ProcessName + "' was killed.");
			}
		}

		internal static Process[] GetAllProcesses()
		{
			return Process.GetProcesses();
		}

		public struct Counter
		{
			private string
				name,
				instance,
				_object;
			private PerformanceCounter perfCounter;

			public string Name => name;
			public string Instance => instance;
			public string Object => _object;

			public Counter(string Name, string Object)
			{
				name = Name;
				_object = Object;
				instance = string.Empty;
				perfCounter = new PerformanceCounter(Object, Name);
			}
			public Counter(string Name, string Object, string Instance)
			{
				name = Name;
				_object = Object;
				instance = Instance;
				perfCounter = new PerformanceCounter(Object, Name, Instance);
			}

			public float GetCounterValue()
			{
				return perfCounter.NextValue();
			}

			public static readonly Counter
				Av_Bytes = new Counter("Available Bytes", "Memory"),
				Av_KBytes = new Counter("Available KBytes", "Memory"),
				Av_MBytes = new Counter("Available MBytes", "Memory"),
				ProcTime = new Counter("% Processor Time", "Processor", "_Total");
		}

		internal static float TrackResource_Float(Counter resourceCounter)
		{
			return resourceCounter.GetCounterValue();
		}
		internal static double TrackResource_Double(Counter resourceCounter)
		{
			return resourceCounter.GetCounterValue();
		}
		internal static double TrackResource_Int(Counter resourceCounter)
		{
			return (int)resourceCounter.GetCounterValue();
		}

		internal static DataGridView GetProcDataView()
		{
			DataGridView procDGV = new DataGridView();
			DataGridViewColumn[] procInfo = new DataGridViewColumn[]
			{
				new DataGridViewColumn { Name = "Name" },
				new DataGridViewColumn { Name = "Id" },
				new DataGridViewColumn { Name = "Handle" },
				new DataGridViewColumn { Name = "MainModule" },
				new DataGridViewColumn { Name = "ModuleFileName" },
			};
			Process[] allProcesses = GetAllProcesses();
			for (int i = 0; i < allProcesses.Length; i++)
			{
				Process process = allProcesses[i];
				procDGV["Name", i].Value = process.ProcessName;
				procDGV["Id", i].Value = process.Id.ToString();
				procDGV["Handle", i].Value = process.Handle.ToString();
				procDGV["MainModule", i].Value = process.MainModule.ModuleName;
				procDGV["FileName", i].Value = process.MainModule.FileName;
			}
			return procDGV;
		}

		internal static Process Create(string FilePath)
		{
			return Process.Start(FilePath);
		}
		internal static Process Create(string FilePath, string CommandLineArguments)
		{
			return Process.Start(FilePath, CommandLineArguments);
		}

		internal static void Start(string FilePath)
		{
			Start(FilePath);
		}
		internal static void Start(string FilePath, string CommandLineArguments)
		{
			Start(FilePath, CommandLineArguments);
		}

		internal static Process StartWithInfo(ProcessStartInfo startInfo)
		{
			return Process.Start(startInfo);
		}
		internal static void CreateWithInfo(ProcessStartInfo startInfo)
		{
			StartWithInfo(startInfo);
		}

		internal static bool ProcessExists(string ProcessName)
		{
			try
			{
				Process process = GetSpecificProc(ProcessName);
				if (process != null) return true;
			}
			catch
			{
				return false;
			}
			return false;
		}
		internal static bool ProcessesExist(string ProcessName)
		{
			try
			{
				Process[] processes = GetSpecificProc_List(ProcessName);
				if (processes != null) if (processes.Length > 0) return true;
			}
			catch
			{
				return false;
			}
			return false;
		}
	}
    
    public class ProcessManager
    {
		public static Process[] GetProcesses(string ProcessName) => Functions.GetSpecificProc_List(ProcessName);
		public static Process GetProcess(string ProcessName) => Functions.GetSpecificProc(ProcessName);

		public static void KillProcesses(string ProcessName) => Functions.KillProc_List(ProcessName);
		public static void KillProcess(string ProcessName) => Functions.KillProc(ProcessName);

		public static Process[] GetAllProcesses(string ProcessName) => Functions.GetAllProcesses();

		public static float TrackAndGetDeviceRes_Float(Functions.Counter resCounter) => resCounter.GetCounterValue();
		public static double TrackAndGetDeviceRes_Double(Functions.Counter resCounter) => resCounter.GetCounterValue();
		public static int TrackAndGetDeviceRes_Int(Functions.Counter resCounter) => (int)resCounter.GetCounterValue();

		public static DataGridView GetDataGridViewOfProcesses() => Functions.GetProcDataView();

		public static Process CreateNewProcess(string FileName) => Functions.Create(FileName);
		public static Process CreateNewProcess(string FileName, string CmdLineArgs) => Functions.Create(FileName, CmdLineArgs);

		public static void StartNewProcess(string FileName) => Functions.Start(FileName);
		public static void StartNewProcess(string FileName, string CmdLineArgs) => Functions.Start(FileName, CmdLineArgs);

		public static Process CreateProcessWithStartInfo(ProcessStartInfo procStartInfo) => Functions.StartWithInfo(procStartInfo);
		public static void StartNewProcessWithStartInfo(ProcessStartInfo procStartInfo) => Functions.CreateWithInfo(procStartInfo);

		public static bool ProcessExists(string ProcName) => Functions.ProcessExists(ProcName);
		public static bool ProcessesExist(string ProcName) => Functions.ProcessesExist(ProcName);
    }
}
