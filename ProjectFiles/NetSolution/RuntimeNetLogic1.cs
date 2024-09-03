#region Using directives
using System;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.HMIProject;
using FTOptix.Retentivity;
using FTOptix.UI;
using FTOptix.NativeUI;
using FTOptix.WebUI;
using FTOptix.CoreBase;
using FTOptix.NetLogic;
using FTOptix.Core;
using FTOptix.RAEtherNetIP;
using FTOptix.CommunicationDriver;
#endregion

public class RuntimeNetLogic1 : BaseNetLogic
{
    PeriodicTask myTask1;
    
    public override void Start()
    {
    //Take a look at the end of RuntimeNetlogic that appears on Login Button Netlogic on
    //UI/LoginForm/LoginFormComponents/Login/LoginButton/LoginButtonLogic.cs
    //To see user access rights
     
       var navigationPanel = Project.Current.Get<NavigationPanel>("UI/MainWindow/NavigationPanel1");
       
       //Disable Navigation Panel before any user is logged
       navigationPanel.Enabled = false;
       //navigationPanel.ChangePanelByTabIndex(2,navigationPanel.NodeId);
       Log.Info("NavigationPanelname:",navigationPanel.BrowseName.ToString());
            
        // Mostrar el usuari logat
        var usuario = Session.User;
        var user = Session.User.BrowseName;
        var mylabel = Project.Current.Get<Label>("UI/MainWindow/Label2");
        mylabel.Text = user.ToString();
        
        //Mostra el grup del usuari
        var userGroups = usuario.Refs.GetObjects(FTOptix.Core.ReferenceTypes.HasGroup, false);
        foreach (var group in userGroups)
        {
            Log.Info("Group:",group.BrowseName);
            var mylabel_group = Project.Current.Get<Label>("UI/MainWindow/Label4");
            mylabel_group.Text = group.BrowseName.ToString();
          
        }
        
        myTask1 = new PeriodicTask(block_panel_on_logout, 1000, LogicObject);
        myTask1.Start();
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
        myTask1.Dispose();
    }

    
    private void block_panel_on_logout()
    {
        var user = Session.User.BrowseName;
        if (user == "Anonymous")
        {
            var navigationPanel = Project.Current.Get<NavigationPanel>("UI/MainWindow/NavigationPanel1");
            navigationPanel.Enabled = false;
        }
    }
}
    
   
