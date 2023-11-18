<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="CalculateRank.aspx.cs" Inherits="BGS.CalculateRank" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="PAdd" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
        <div class="headertxt pagePannelHeader">
            CALCULATE RANK
        </div>
    </asp:Panel>
    <br />
    <div class="row pagePannelHeader">
        <table>
            <tr>
                <td>
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="submitbutton" OnClick="SubmitRank" Enabled="false" OnClientClick='return confirm("Are you sure ? do you want to submit because after submit you can not change this quarter data");' />
                    <asp:Button ID="btnexportToExcel" runat="server" CssClass="submitbutton" Text="Export to excel" OnClick="ExportToExcel" />
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="submitbutton" OnClick="Back" />
                </td>
            </tr>
        </table>
    </div>
    <br />
    <div>
        <asp:GridView ID="gvCalculateRank" runat="server" EmptyDataText="No Row Found." Width="100%" AutoGenerateColumns="false" AlternatingRowStyle-CssClass="alt" CssClass="mGrid"
            ShowHeaderWhenEmpty="True">
            <Columns>
                <asp:BoundField HeaderText=" Quarter Name" DataField="Name" />
                <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />
                <asp:BoundField HeaderText="Score" DataField="Score" />
<%--                <asp:BoundField HeaderText="Rank" DataField="Rank" />--%>
                <asp:TemplateField  HeaderText="Rank">
                    <ItemTemplate>
                        <asp:Label ID="lblRank" runat="server" Text='<%#Eval("Rank") %>'></asp:Label>
                        <asp:HiddenField ID="hfBrokerNameID" runat="server" Value='<%#Eval("BrokerNameID")%>' />
                        <asp:HiddenField ID="hfQuarterID" runat="server" Value='<%#Eval("QuarterID")%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <br />
    <br />
</asp:Content>
