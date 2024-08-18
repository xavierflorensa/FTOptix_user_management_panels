#region Using directives
using System;
using FTOptix.CoreBase;
using FTOptix.HMIProject;
using UAManagedCore;
using OpcUa = UAManagedCore.OpcUa;
using FTOptix.NetLogic;
using FTOptix.Core;
using FTOptix.UI;
#endregion

public class LoginButtonLogic : BaseNetLogic
{
    public override void Start()
    {
        ComboBox comboBox = Owner.Owner.Get<ComboBox>("Username");
        if (Project.Current.Authentication.AuthenticationMode == AuthenticationMode.ModelOnly)
        {
            comboBox.Mode = ComboBoxMode.Normal;
        }
        else
        {
            comboBox.Mode = ComboBoxMode.Editable;
        }
    }

    public override void Stop()
    {

    }

    [ExportMethod]
    public void PerformLogin(string username, string password)
    {
        
        var usersAlias = LogicObject.GetAlias("Users");
        if (usersAlias == null || usersAlias.NodeId == NodeId.Empty)
        {
            Log.Error("LoginButtonLogic", "Missing Users alias");
            return;
        }

        var passwordExpiredDialogType = LogicObject.GetAlias("PasswordExpiredDialogType") as DialogType;
        if (passwordExpiredDialogType == null)
        {
            Log.Error("LoginButtonLogic", "Missing PasswordExpiredDialogType alias");
            return;
        }

        Button loginButton = (Button)Owner;
        loginButton.Enabled = false;

        try
        {
            var loginResult = Session.Login(username, password);
            if (loginResult.ResultCode == ChangeUserResultCode.PasswordExpired)
            {
                loginButton.Enabled = true;
                var user = usersAlias.Get<User>(username);
                var ownerButton = (Button)Owner;
                ownerButton.OpenDialog(passwordExpiredDialogType, user.NodeId);
                return;
            }
            else if (loginResult.ResultCode != ChangeUserResultCode.Success)
            {
                loginButton.Enabled = true;
                Log.Error("LoginButtonLogic", "Authentication failed");
            }

            if (loginResult.ResultCode != ChangeUserResultCode.Success)
            {
                var outputMessageLabel = Owner.Owner.GetObject("LoginFormOutputMessage");
                var outputMessageLogic = outputMessageLabel.GetObject("LoginFormOutputMessageLogic");
                outputMessageLogic.ExecuteMethod("SetOutputMessage", new object[] { (int)loginResult.ResultCode });
            }
        }
        catch (Exception e)
        {
            Log.Error("LoginButtonLogic", e.Message);
        }

        loginButton.Enabled = true;
        //Start Custom behaviour 
         // Mostrar el usuari logat
       var usuario = Session.User;
       var usuari = Session.User.BrowseName;
       var mylabel = Project.Current.Get<Label>("UI/MainWindow/Label2");
       mylabel.Text = usuari.ToString();
       //Mostra el grup del usuari
       var navigationPanel = Project.Current.Get<NavigationPanel>("UI/MainWindow/NavigationPanel1");
       //Mostra el grup del usuari
       var userGroups = usuario.Refs.GetObjects(FTOptix.Core.ReferenceTypes.HasGroup, false);
       if (usuari== "Anonymous")
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
      
        //End custom behaviour
    }
}
