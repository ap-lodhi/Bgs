<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master" AutoEventWireup="true" CodeBehind="weightMaster.aspx.cs" Inherits="BGS.weightMaster" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        .form-group .row table td
        {
            padding-left: 50px;
        }

        .pagePannelHeader
        {
            padding-left: 30px;
        }
    </style>
    <div>
        <div class="form-group">

            <div class="row">
                <%-- <asp:Panel ID="PAdd" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
                    <fieldset id="Fieldset1" runat="server" style="background-position: center; font-family: 'Times New Roman'" class="headertxt pagePannelHeader">ADD WEIGHT MASTER </fieldset>
                </asp:Panel>
                <br />
                <table align="center" class="form-group">


                    <tr align="center">
                        <td>User Name:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlUserName" runat="server" CssClass="form-control"></asp:DropDownList>
                        </td>
                        <td>Role:
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control"></asp:DropDownList>
                        </td>

                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                    <tr align="center">
                        <td>Department:
                        </td>
                        <td>
                            <asp:TextBox ID="txtDep" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </td>
                        <td>Weight:
                        </td>
                        <td>
                            <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <td colspan="4"></td>
                    </tr>
                    <tr>
                        <td colspan="4" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="Submit" />
                            &nbsp;
                           <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
                        </td>

                    </tr>


                </table>


                <br />--%>

                <asp:Panel ID="PNLWeight_Mater" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">
                    <fieldset id="Fieldset2" runat="server" style="background-position: center; font-family: 'Times New Roman'" class="headertxt pagePannelHeader">WEIGHT MASTER  DETAILS</fieldset>


                    <asp:GridView ID="gvWeightMater" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found." ShowHeaderWhenEmpty="True">

                        <Columns>
                            <asp:BoundField HeaderText="User Name" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="Middle" DataField="EmpName" />
                            <asp:BoundField HeaderText="Role" DataField="Role" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="Middle" />
                            <asp:TemplateField HeaderText="Weight" HeaderStyle-Font-Bold="true" HeaderStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtWeight" runat="server" Text='<%# Eval("Weight") %>' ReadOnly='<%# !string.IsNullOrEmpty(Convert.ToString(Eval("Weight"))) %>'></asp:TextBox>
                                    <asp:HiddenField ID="hfWeightId" runat="server" Value='<%# Eval("WeightId") %>' />
                                    <asp:HiddenField ID="hfEmpId" runat="server" Value='<%# Eval("EMPLID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <ItemTemplate>
                                    <asp:Button ID="btnEdit" runat="server" Text="Edit"  OnClick="UpdateWeightMaster" CommandArgument='<%# Eval("WeightId") %>'/>
                                    <asp:Button ID="btnDelete" runat="server" Text="Delete" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                    </asp:GridView>
                    <br />
                    <br />
                    <div>
                        <asp:Button ID="btnSave" runat="server" Text="Save" />
                        <asp:Button ID="btnCencel" runat="server" Text="Cancel" />
                    </div>
                </asp:Panel>
                <br />

                <div>
                </div>
                <%--
<table border="1" cellpadding="2" cellspacing="2" >
    <tr>
    <th>User Name </th>
       
     <th>Role</th>
     <th>Department</th>
      <th>Wts</th>
      <th></th>
      </tr>
    
    <tr>
    <td>USER 1</td>
     <td>Fund Manager</td>
     <td>SALE</td>
      <td> <input id="Text1" type="text" /></td>
      <td>
        <input id="Button1" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td>USER 2</td>
     <td>Reseach Analyst</td>
     <td> MARKETING</td>
      <td> <input id="Text2" type="text" /></td>
      <td>
        <input id="Button2" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td> USER 3</td>
     <td>Reseach Analyst</td>
     <td> OTHER</td>
      <td> <input id="Text3" type="text" /></td>
      <td>
        <input id="Button3" type="button" value="Edit" />
        </td>
       
    </tr>
    <tr>
    <td> USER 3</td>
     <td>Reseach Analyst</td>
     <td>SALES</td>
      <td> <input id="Text4" type="text" /></td>
      <td>
        <input id="Button5" type="button" value="Edit" />
        </td>
       
    </tr>
    <tr>
    <td>USER 4</td>
     <td>Reseach Analyst</td>
     <td>OTHER</td>
      <td> <input id="Text5" type="text" /></td>
      <td>
        <input id="Button6" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td>USER 5</td>
     <td>Reseach Analyst</td>
     <td>OTHER</td>

      <td> <input id="Text6" type="text" /></td>
      <td>
        <input id="Button7" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td>USER 6</td>
     <td>Reseach Analyst</td>
     <td>MARKETING</td>
      <td> <input id="Text8" type="text" /></td>
      <td>
        <input id="Button9" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td>USER 7</td>
     <td>Reseach Analyst</td>
     <td>SALES</td>
      <td> <input id="Text7" type="text" /></td>
      <td>
        <input id="Button8" type="button" value="Edit" />
        </td>
                
    </tr>
    <tr>
    <td></td>
    <td><td> <asp:Button ID="Button11" runat="server" Text="ADD" Height="34px" 
             Width="80px" /></td></td>
    <td> <asp:Button ID="Button10" runat="server" Text="SUBMIT" Height="34px" 
             Width="80px" /></td>
   
     <td> <asp:Button ID="Button4" runat="server" Text="CANCLE" Height="34px" 
             Width="80px" /></td>
     </tr>
        </table>
   
                --%>
            </div>
        </div>


    </div>
</asp:Content>
