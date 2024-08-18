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
    public override void Start()
    {
       
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
        
       
    }

    public override void Stop()
    {
        // Insert code to be executed when the user-defined logic is stopped
    }
    [ExportMethod]
    
    public void Update()
    {
       Actualitza();
       Log.Info("Updating: ");
    }

    
    
   
    
     private void Actualitza()
    {
       // Mostrar el usuari logat
       var usuario = Session.User;
       var user = Session.User.BrowseName;
       var mylabel = Project.Current.Get<Label>("UI/MainWindow/Label2");
       mylabel.Text = user.ToString();
       //Mostra el grup del usuari
       var navigationPanel = Project.Current.Get<NavigationPanel>("UI/MainWindow/NavigationPanel1");
       //Mostra el grup del usuari
       var userGroups = usuario.Refs.GetObjects(FTOptix.Core.ReferenceTypes.HasGroup, false);
       if (user== "Anonymous")
       {
        navigationPanel.Enabled = false;
       }
       foreach (var group in userGroups)
       {
            Log.Info("User Group: ",group.BrowseName);
            var mylabel_group = Project.Current.Get<Label>("UI/MainWindow/Label4");
            mylabel_group.Text = group.BrowseName.ToString();
            //Make Navigation Panel visible if rigt usergroup is logged
            var  panelPC = navigationPanel.Panels[1];
            var panelTrends = navigationPanel.Panels[2];
            var panelDatabase = navigationPanel.Panels[3];
            if (group.BrowseName.ToString()=="Technician")
            {
                navigationPanel.Enabled = true;
                panelPC.Enabled = false;
                panelTrends.Enabled = true;
                panelDatabase.Enabled = true;
            }
            else 
            {
                if (group.BrowseName.ToString()=="Administrator")
                { 
                navigationPanel.Enabled = true;
                panelPC.Enabled = true;
                panelTrends.Enabled = false;
                panelDatabase.Enabled = true;
                }
            else 
                {
                navigationPanel.Enabled = false;
                }
            
            }
       
       } 
    }
}
    
   
