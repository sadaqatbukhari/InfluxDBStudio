using System.Runtime.InteropServices;

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components. If you need to access a type in this assembly from COM,
// set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The GUID of the type library if this project is exposed to COM.
[assembly: Guid("0578e050-27c9-4345-a6c9-ebaa887611d2")]

// Enable log4net using app.config.
[assembly: log4net.Config.XmlConfigurator(Watch = true)]
