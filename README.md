# GodotIpLogger       

an independent logging for ipconnections. in some cases good if you have your own log, for example if host is stunned by dosattack or something.    
in C# with Godot 4.1 for Windows    
it can log with gdprint and in a logfile with datename. select with dateformat in classdeclaration.    


# Installation and Usage:    
Install - copy the class IpInfoClass.cs to your project.    

Use - like shown in the demoproject in ipinformation.cs you have first to instantiate the class IpInfoClass in your project.    
      with parameter 1 loggingtofile (true or false)     
           parameter 2 logging to screen with gdprint to godotoutput (true or false)     
           parameter 3 dateformat as string for example US-format "MM-dd-yyyy"    
example:  public IpInfoClass ipic = new IpInfoClass(true, true, "dd-MM-yyyy");    
      then you can start the logging by calling the method startinformation() example ipic.startinformation();    
      the start is optimal placed in your _Ready() method.     
      if you want to log new ipconnections you have to call method ChangedConnections()    
      for example timed in _Process()    
```
      public override void _Process(double delta)    
      {    
         time = time + delta;    
         if (time > inspectioninterval)    
         {    
            ipic.ChangedConnections();    
            time = 0;    
         }    
     }    
```



# Last changes:    
- v0.1 start

# contact:
sys_temerror at web dot de    
![Pic1](systemerror.JPG)
