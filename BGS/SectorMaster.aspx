<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="UserMaster.aspx.cs" Inherits="BGS.UserMaster_Page" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-group .row table td {
            padding-left: 30px;
        }

        .pagePannelHeader {
            padding-left: 30px;
        }

        .pageButtion {
            padding-left: 145px;
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
    <div>
        <asp:Panel ID="pnlBroker" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
            <div class="headertxt pagePannelHeader">
                Sector Master
            </div>
        </asp:Panel>
        <div class="form-group">
            <div class="row pagePannelHeader">
                <table id="tblsearch" runat="server">
                    <tr>
                        <td>
                            <asp:Label ID="lblSectorName" runat="server" Text="Sector Name" Font-Bold="true"></asp:Label>
                        </td>
                        <td>

                            <asp:TextBox ID="txtSectorName" runat="server" Width="200px" CssClass="textbox" Height="30px" Font-Size="Larger"></asp:TextBox>

                        </td>
                        <td>
                            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="Save" CssClass="button" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="form-group">
            <div class="row pagePannelHeader">
                <asp:GridView ID="gvSectorName" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found." AllowPaging="True" PageSize="10" Width="100%"
                    ShowHeaderWhenEmpty="True" Font-Size="Smaller" HeaderStyle-Font-Size="Medium" CellPadding="5" CellSpacing="3" OnPageIndexChanging="BrokerPageIndexChanging" EmptyDataRowStyle-HorizontalAlign="Center">
                    <PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PageButtonCount="2" PreviousPageText="Previous" />
                    <HeaderStyle BackColor="#425290" ForeColor="White" />
                    <Columns>
                        <asp:TemplateField HeaderText="Sector Name" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSectorName" runat="server" Text='<%# Eval("SectorName") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="EditSector" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName='<%#Eval("SectorID") %>' />
                                <%--                        <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%#Eval("BrokerID") %>' OnClientClick='return confirm("Do you want to delete this broker ?");' OnClick="DeleteBrokerMaster" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
