<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ScreenAllocation.ascx.cs" Inherits="BGS.ScreenAllocation" %>
<style>
    .form-group .row table td {
        padding-left: 30px;
    }

    .pagePannelHeader {
        padding-left: 15px;
    }

    .pageButtion {
        padding-left: 110px;
    }

    .gvHeadertxtAlign {
        text-align: center;
        width: 150px;
    }

    .gvActionHeadertxtAlign {
        text-align: center;
        width: 88px;
    }
</style>
<div class="module" style="left: 50%; width: 80%;">
    <br />
    <asp:Panel ID="pnlBroker" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
        <div class="headertxt pagePannelHeader">
            Broker/Firm Master
        </div>
    </asp:Panel>

    <table border="0" cellpadding="5" cellspacing="5" width="100%" style="margin-top: 10px;">
        <tr>
            <td align="left" class="pagePannelHeader" >
                <asp:Button ID="Button1" Text="Submit" CssClass="submitbutton" OnClick="AssignScreenToUser" runat="server" />
            </td>
            <td align="right" class="pagePannelHeader">
                <asp:Button ID="Button2" Text="Back" CssClass="submitbutton" OnClick="BackButton" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="pagePannelHeader">Screen alocated to : <b>
                <asp:Label ID="lblUserName" runat="server" /></b>
            </td>
        </tr>
        <tr>
            <td class="pagePannelHeader" style="width:100%">
                <asp:GridView ID="grdScreens" runat="server" AutoGenerateColumns="False" AlternatingRowStyle-CssClass="alt"
                    RowStyle-HorizontalAlign="Center" Width="100%" ForeColor="Black" DataKeyNames="ScreenId">
                    <HeaderStyle BackColor="#425290" ForeColor="White" />
                    <RowStyle HorizontalAlign="Center"></RowStyle>
                    <Columns>
                        <asp:TemplateField HeaderText="Select">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkSelectAll" runat="server" AutoPostBack="true" Text="Select"
                                    Font-Size="Small" ForeColor="White" onclick="javascript:HeaderClick(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--<span><%# Eval("IsActive").ToString() %></span>--%>
                                <asp:CheckBox ID="chkselect" runat="server" Checked='<%#Convert.ToBoolean(!string.IsNullOrEmpty(Eval("IsActive").ToString())?Eval("IsActive"):false)%>' />
                                <asp:HiddenField ID="hfScreenId" runat="server" Value='<%#Eval("SCREENID") %>' />
                                <asp:HiddenField ID="hfModuleId" runat="server" Value='<%#Eval("ModuleID") %>' />
                                <asp:HiddenField ID="hfScreenAssigned" runat="server" Value='<%#Eval("ScreenAssignId") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Module Name">
                            <ItemTemplate>
                                <asp:Label ID="lblModuleName" runat="server" Text='<%# Eval("ModuleName") %>' Font-Size="small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Screens">
                            <ItemTemplate>
                                <asp:Label ID="lblScreens" runat="server" Text='<%# Eval("SCREENNAME") %>' Font-Size="small"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <AlternatingRowStyle CssClass="alt"></AlternatingRowStyle>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <br />
                <br />
            </td>
        </tr>
    </table>
</div>
<script type="text/javascript">
    function HeaderClick(CheckBox) {
        //Get target base & child control.
        var TotalChkBx = 0;
        var TargetBaseControl = document.getElementById('<%= this.grdScreens.ClientID %>');
        var TargetChildControl = "chkselect";

        //Get all the control of the type INPUT in the base control.
        var Inputs = TargetBaseControl.getElementsByTagName("input");

        //Checked/Unchecked all the checkBoxes in side the GridView.
        for (var n = 0; n < Inputs.length; ++n)
            if (Inputs[n].type == 'checkbox' &&
                Inputs[n].id.indexOf(TargetChildControl, 0) >= 0)
                Inputs[n].checked = CheckBox.checked;

        //Reset Counter
        Counter = CheckBox.checked ? TotalChkBx : 0;
    }
</script>
