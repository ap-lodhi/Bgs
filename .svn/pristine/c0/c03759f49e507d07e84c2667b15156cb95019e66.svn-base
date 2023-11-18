<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="BGS.ChangePassword" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        .pagePannelHeader {
            padding-left: 30px;
        }
    </style>
      <div class="module">
            <%--<center>
                    <h3>Broker/Firm Master</h3>
                </center>--%>
        </div>
    <div>
        <asp:Panel ID="PAdd" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
            <div class="headertxt pagePannelHeader">
                Change Password
            </div>
        </asp:Panel>
    </div>

    <div class="form-group"
        style="position: absolute; top: 156px; left: 30%; height: 250px; width: 500px; border-style: solid; border-width: thin;">
        <div style="width: 100%;">
            <br />
            <div style="padding-left: 70px">
                <table style="width: 100%">
                    <tr>
                        <td style="width: 120px">User Id </td>
                        <td style="width: 5px">:</td>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtEmpId" runat="server"  ></asp:TextBox><br />

                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvuserName" runat="server"
                                ErrorMessage="*" ControlToValidate="txtEmpId"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 120px">Old Password  </td>
                        <td style="width: 5px">:</td>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtOldPwd" runat="server" TextMode="Password"></asp:TextBox><br />

                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvOldPwd" runat="server"
                                ErrorMessage="*" ControlToValidate="txtOldPwd"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 120px">New Password  </td>
                        <td style="width: 5px">:</td>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtNewPwd" runat="server" TextMode="Password"></asp:TextBox><br />

                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvNewPwd" runat="server"
                                ErrorMessage="*" ControlToValidate="txtNewPwd"
                                Display="Dynamic" ForeColor="Red" SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 120px">Confirm Password  </td>
                        <td style="width: 5px">:</td>
                        <td style="width: 150px">
                            <asp:TextBox ID="txtConfirmPwd" runat="server" TextMode="Password"></asp:TextBox><br />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="rfvConfirmPwd" runat="server"
                                ErrorMessage="*"
                                ControlToValidate="txtConfirmPwd" Display="Dynamic" ForeColor="Red"
                                SetFocusOnError="True" ValidationGroup="Submit"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmvConfirmPwd" runat="server"
                                ControlToCompare="txtNewPwd" ControlToValidate="txtConfirmPwd"
                                Display="Dynamic" ErrorMessage="*"
                                ForeColor="Red" SetFocusOnError="True" ValidationGroup="Submit"></asp:CompareValidator>
                        </td>
                    </tr>
                </table>
                <br />
                <table style="width: 100%">
                    <tr>
                        <td style="width: 120px">&nbsp;</td>
                        <td>&nbsp;
                        </td>
                        <td>
                            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="submitbutton" Width="80px" OnClick="UserChangePassword" ValidationGroup="Submit" />&nbsp;&nbsp;
                                  <asp:Button ID="btnBack" runat="server" Text="Cancel" CssClass="submitbutton" Width="80" OnClick="Back" CausesValidation="false" />
                        </td>
                        <td style="text-align:left;"></td>
                    </tr>

                </table>

            </div>
        </div>
    </div>
</asp:Content>
