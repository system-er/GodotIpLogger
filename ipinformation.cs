using Godot;
using System;


public partial class ipinformation : Node2D
{
    public static double time;
    public static float inspectioninterval;
    
    //first parameter filelogging
    //second parameter print with GD.Print
    //third parameter is the date format
    public IpInfoClass ipic = new IpInfoClass(true, true, "dd-MM-yyyy");


    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        ipic.startinformation();
        inspectioninterval = 1.0f;
    }


    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        time = time + delta;
        if (time > inspectioninterval)
        {
            ipic.ChangedConnections();
            time = 0;
        }
    }

}
