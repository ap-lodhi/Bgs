<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="QuarterMaster.aspx.cs" Inherits="BGS.QuarterMaster" %>
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
    <div>
        <asp:Panel ID="pnlBroker" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
            <div class="headertxt pagePannelHeader">
                Quarter Master
            </div>
        </asp:Panel>

        <br />
        <div class="form-group">
            <div class="row pagePannelHeader">

                <table>
                    <tr>

                        <td>Name&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:TextBox ID="txtQuarterName" CssClass="txtbox" runat="server" ValidationGroup="QuarterMaster"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" ErrorMessage="*" CssClass="errorMessage"  ControlToValidate="txtQuarterName" InitialValue="" runat="server" ValidationGroup="QuarterMaster" />
                        </td>
                        <td>Year&nbsp;&nbsp;&nbsp;</td>
                        <td>
                            <asp:DropDownList ID="ddlYear" CssClass="dropdowncss" runat="server" Width="250px" ValidationGroup="QuarterMaster"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvYear" ErrorMessage="*" CssClass="errorMessage"  ControlToValidate="ddlYear" InitialValue="" runat="server" ValidationGroup="QuarterMaster" />
                        </td>

                    </tr>

                </table>
            </div>
        </div>
        <div class="form-group">
            <div class="row pageButtion">
                <table>
                    <tr align="center">
                        <td colspan="3">
                            <asp:Button Text="Save" ID="btnSave" runat="server" OnClick="SaveQuarter" ValidationGroup="QuarterMaster" CssClass="inputbutton" />
                            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CssClass="inputbutton" OnClick="CancelQuarter" />
                        </td>

                    </tr>
                </table>
            </div>
        </div>
          <asp:Panel ID="PNLQuarter" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
        <fieldset id="FsQuarter" runat="server" style="background-position: center; font-family: 'Times New Roman'" class="headertxt pagePannelHeader">Quarter Master  Details</fieldset>
    </asp:Panel>
    <br />
        <div class="pagePannelHeader">
       
            <asp:GridView ID="gvQuarterMaster" runat="server" AutoGenerateColumns="false" EmptyDataText="No records found."   AllowPaging="True" PageSize="10"
                ShowHeaderWhenEmpty="True"  Font-Size="Smaller" HeaderStyle-Font-Size="Medium" CellPadding="5" EmptyDataRowStyle-HorizontalAlign="Center" Width="100%"  CellSpacing="3"  OnPageIndexChanging="QuarterPageIndexChanging">
                 <PagerSettings Mode="NextPreviousFirstLast"  NextPageText="Next" PageButtonCount="2" PreviousPageText="Previous" />
                <HeaderStyle BackColor="#425290" ForeColor="White" />
                <Columns>
                    <asp:BoundField HeaderText="Quarter Name" DataField="Name" ItemStyle-HorizontalAlign="Center"   HeaderStyle-CssClass="gvHeadertxtAlign" HtmlEncode="false"/>
                    <asp:BoundField HeaderText="Year" DataField="QuarterYear" HtmlEncode="false" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvHeadertxtAlign"/>
                    <asp:TemplateField HeaderText="Action"  ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="gvActionHeadertxtAlign" >
                        <ItemTemplate>
                            <asp:Button ID="btnEdit" runat="server" Text="Edit" OnClick="EditQuarter" CommandArgument="<%# ((GridViewRow)Container).RowIndex%>" CommandName='<%#Eval("QuarterID") %>' />
                            <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandArgument='<%#Eval("QuarterID") %>' OnClientClick='return confirm("Do you want to delete this Quarter ?");' OnClick="DeleteQuarter" />                       
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
    </div>
    </div>
</asp:Content>
