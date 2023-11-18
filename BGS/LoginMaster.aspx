<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMaster.master" AutoEventWireup="true" CodeBehind="LoginMaster.aspx.cs" Inherits="BGS.LoginMaster_Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">


    <script type="text/javascript" language="javascript">

        function isNumericKeyStroke() {
            var returnValue = false;
            var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

            if (((keyCode >= 48) && (keyCode <= 57)) || ((keyCode >= 96) && (keyCode <= 105)) || // All numerics
                      (keyCode == 8) || // Backspace
                      (keyCode == 9) ||   //TaB code
                      (keyCode == 13))     // Carriage Return
                returnValue = true;

            if (window.event.returnValue)
                window.event.returnValue = returnValue;

            return returnValue;
        }

    </script>
    <div class="module"
        style="position: absolute; top: 156px; left: 17%; height: 208px; width: 500px">

        <div style="border: 1px solid #dadada;">
            <div>
                <br />
                <center>
                    <h3>Broker Grading System </h3>
                </center>
                <br />
                 <asp:Label ID="lblErrorMsg" runat="server" Text=""></asp:Label><br />
                <br/>
                <center>
                    <table>
                        <tr>
                            <td style="text-align: right" valign="top">
                                <asp:Label ID="Label1" runat="server" Text="Login ID" CssClass="LabelDetails"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoginID" runat="server" CssClass="TextBoxcont"
                                    MaxLength="50" TabIndex="1" Width="250px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <asp:Label ID="Label2" runat="server" Text="Password" CssClass="LabelDetails" MaxLength="20"></asp:Label>
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="TextBoxcont"
                                    TextMode="Password" TabIndex="2" Width="250px"></asp:TextBox><br />
                            </td>
                        </tr>
                        <tr>
                             <td valign="top">
                                 </td>
                            <td style="text-align: left">
                                <asp:Image ID="captchaImage" runat="server" Height="40px" Width="150px" top="20" ImageUrl="~/MyCaptcha.aspx" />
                                </td>
                            </tr>
                        <tr>
                              <td valign="top">
                                  </td>
                            <td style="text-align: left">
                                <br />
                                <asp:TextBox ID="captchacode" runat="server" Height="30" Placeholder="Enter Captcha code"></asp:TextBox>
                            </td>
                           

                        </tr>
                        <tr>
                            <td colspan="2" align="justify">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
   <asp:Button ID="BtnSubmit" runat="server" Text="Login" CssClass="submitbutton" Height="32px"
       OnClick="BtnSubmit_Click" TabIndex="3"
       Width="70px" />
                            </td>
                        </tr>
                    </table>
                    <br />
                    <a id="img" runat="server" href="ForgetPwd.aspx">ForgotPassword</a>
                    <br />

                </center> 

            </div>
        </div>
    </div>

</asp:Content>
