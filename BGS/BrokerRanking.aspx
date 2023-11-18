<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="BrokerRanking.aspx.cs" Inherits="BGS.BrokerRanking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="pagePannelHeader">
        <br />
        <br />

        <asp:Button ID="btnExcel" runat="server" Text="Export to excel" CssClass="submitbutton" OnClick="btnExcel_Click" />
        &nbsp;&nbsp;
                    
         <asp:Button ID="btnpdf" runat="server" Text="Export to pdf" CssClass="submitbutton" OnClick="btnpdf_Click" />
        &nbsp;&nbsp;
                   
        <asp:GridView ID="gvBrokerEmailList" runat="server" EmptyDataText="No Row Found." AutoGenerateColumns="false"
            HeaderStyle-HorizontalAlign="Center" EmptyDataRowStyle-HorizontalAlign="Center" HorizontalAlign="Center"
            ShowHeaderWhenEmpty="True" RowStyle-HorizontalAlign="Center" CssClass="mGrid" AlternatingRowStyle-CssClass="alt" Width="100%">
            <Columns>
                <asp:TemplateField HeaderText="Quarter Name">
                    <ItemTemplate>
                        <asp:Label ID="lblquarter" runat="server" Text='<%# Eval("QuarterName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Broker Name">
                    <ItemTemplate>
                        <asp:Label ID="lblbroker" runat="server" Text='<%# Eval("BrokerName") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Score">
                    <ItemTemplate>
                        <%--<asp:Label ID="lblscore" runat="server" Text='<%#Math.Ceiling(Convert.ToDecimal(Eval("TotalScores"))) %>'></asp:Label>--%>
                        <asp:Label ID="lblscore" runat="server" Text='<%#Eval("TotalScores") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rank">
                    <ItemTemplate>
                        <asp:Label ID="lblRank" runat="server" Text='<%# Eval("Rank") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Rank Group">
                    <ItemTemplate>
                        <asp:Label ID="lblRankGroup" runat="server" Text='<%# string.Format("Group {0}",Eval("RankGroup").ToString()) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
