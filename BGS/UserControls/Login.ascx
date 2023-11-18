<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="BGS.UserControls.Login" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<ajaxToolkit:ToolkitScriptManager runat="Server" EnableScriptGlobalization="true"
    EnableScriptLocalization="true" ID="ScriptManager1" ScriptMode="Debug" CombineScripts="false" />
     <asp:UpdatePanel ID="UpdatePanel1" runat="server">
 
            <ContentTemplate>  
<!-- Start: login-holder -->
<div id="login-holder">
    <!-- start logo -->
    <div id="logo-login">
        <table>
            <tr>
                <td valign="top">
                     
                </td>
                <td valign="middle">
                    <asp:Label ID="lblPharam" runat="server" Text="BGS" ForeColor="Black"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <!-- end logo -->
    <div class="clear">
    </div>
    <!--  start loginbox ................................................................................. -->
    <div id="loginbox">
        <!--  start login-inner -->
        <div id="login-inner">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <th>
                    </th>
                    <td>
                        <asp:Label ID="lblErrMsg" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>
                        Username
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtuser" class="login-inp" />
                    </td>
                </tr>
                <tr>
                    <th>
                        Password
                    </th>
                    <td>
                        <asp:TextBox runat="server" ID="txtpwd" onfocus="this.value=''" class="login-inp"
                            TextMode="Password" />
                    </td>
                </tr>                
                <tr>
                    <th>
                    </th>
                    <td valign="top">
                        <asp:CheckBox class="checkbox-size" runat="server" ID="logincheck" />
                        <label for="login-check">
                            Remember me</label>
                    </td>
                </tr>
                <tr>
                    <th>
                    </th>
                    <td>
                        <asp:Button runat="server" ID="btnlogin" class="submit-login" 
                            onclick="btnlogin_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <!--  end login-inner -->
        <div class="clear">
        </div>
        <%--<a href="" class="forgot-pwd">Forgot Password?</a>--%>
    </div>
    <!--  end loginbox -->
    <!--  start forgotbox ................................................................................... -->
    
    <!--  end forgotbox -->
</div>
<!-- End: login-holder -->
</ContentTemplate> 
 </asp:UpdatePanel> 