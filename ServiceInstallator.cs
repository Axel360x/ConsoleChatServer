using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration.Install;
using System.ComponentModel;
using System.ServiceProcess;


namespace Server
{
	[RunInstaller(true)]
	public class ServiceInstallator : Installer
	{

		public ServiceInstallator ()
		{
			var processInstaller = new ServiceProcessInstaller();
			var serviceInstaller = new ServiceInstaller();

			//set the privileges
			processInstaller.Account = ServiceAccount.User;

			serviceInstaller.DisplayName = "ComServer";
			serviceInstaller.StartType = ServiceStartMode.Manual;

			//must be the same as what was set in Program's constructor
			serviceInstaller.ServiceName = "ComServer";

			this.Installers.Add(processInstaller);
			this.Installers.Add(serviceInstaller);


		}
	}
}

