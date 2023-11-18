<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/MasterPage.master"

    AutoEventWireup="true" CodeBehind="BrokerMaster.aspx.cs" EnableEventValidation="false"

    Inherits="BGS.BrokerMaster" %>

 

<%@ Import Namespace="BGS" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript">

        function ClientItemSelected(sender, e) {

            $get("<%=hfBrokerNameID.ClientID %>").value = e.get_value();

        }

    </script>

    <%--<script type="text/javascript">

        function ClientFirmItemSelected(sender, e) {

            $get("<%=hfFirmNameID.ClientID %>").value = e.get_value();

        }

    </script>--%>

    <%--<script type="text/javascript">

        $(function () {

            debugger;

            $("[id$=txtBrokerName]").autocomplete({

                source: function (request, response) {

                    $.ajax({

                        url: '../WebService/AutoComplete.asmx/GetBrokerName',

                        data: "{ 'prefix': '" + request.term + "'}",

                        dataType: "json",

                        type: "POST",

                        contentType: "application/json; charset=utf-8",

                        success: function (data) {

                            response($.map(data.d, function (item) {

                                return {

                                    label: item.split('-')[0],

                                    val: item.split('-')[1]

                                }

                            }))

                        },

                        error: function (response) {

                            alert(response.responseText);

                        },

                        failure: function (response) {

                            alert(response.responseText);

                        }

                    });

                },

                select: function (e, i) {

                    $("[id$=hfBrokerNameID]").val(i.item.val);

                },

                minLength: 1

            });

        });

    </script>--%>

    <div>

        <asp:HiddenField runat="server" ID="hfBrokerNameID" />

        <asp:HiddenField runat="server" ID="hfFirmNameID" />

        <%-- <asp:Panel ID="pnlBroker" runat="server" BorderColor="Silver" BorderWidth="1" HorizontalAlign="Center">

           

        </asp:Panel>--%>

        <div class="module">

            <%--<center>

                    <h3>Broker/Firm Master</h3>

                </center>--%>

        </div>

    </div>

    <br />

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">

        <ContentTemplate>

            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">

                <ProgressTemplate>

                    <div class="loader">

                        <div class="center">

                            <img alt="" src="Images/loader.gif" />

                        </div>

                    </div>

                </ProgressTemplate>

            </asp:UpdateProgress>

            <table class="table">

                <tr>

                    <td id="tdsearch" runat="server">

                        <table>

                            <tr>

                                <td>

                                    <asp:Label ID="lblSearch" runat="server" Text="Search" Font-Bold="true"></asp:Label>

                                </td>

                                <td>&nbsp;

                                </td>

                                <td>

                                    <asp:TextBox ID="txtSerach" runat="server" CssClass="textbox" Placeholder="Search Broker / Firm Name"

                                        OnTextChanged="txtSerach_TextChanged" Width="380px" AutoPostBack="true"></asp:TextBox>

                                    <cc1:AutoCompleteExtender ServiceMethod="BrokerSearch" ServicePath="~/WebService/AutoComplete.asmx"

                                        MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="true" CompletionSetCount="5"

                                        TargetControlID="txtSerach" ID="BrokerSearch" runat="server">

                                    </cc1:AutoCompleteExtender>

                                </td>

                                <td>&nbsp;

                                </td>

                                <td>

                                    <asp:Button ID="btnRset" runat="server" Text="Reset" OnClick="Reset" CssClass="submitbutton" />

                                </td>

                            </tr>

                        </table>

                    </td>

                    <td>

                        <table>

                            <tr>

                                <td>

                                    <asp:Button ID="btnUpload" Text="Upload File" runat="server" CssClass="submitbutton"

                                        Width="100" OnClick="btnUpload_Click" />&nbsp;&nbsp;&nbsp;

                                </td>

                                <td>

                                    <asp:Button ID="btnDownload" Text="Download Template" runat="server" CssClass="submitbutton"

                                        Width="100" OnClick="btnDownload_Click" />&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;

                                </td>

                                <td>

                                    <asp:Button ID="btnBroker" Text="Add New User" runat="server" OnClick="AddBroker"

                                        CssClass="submitbutton" Width="130" />&nbsp;&nbsp;&nbsp;

                                </td>

                                <td style="top:25%;">

                                    <%--<asp:HyperLink NavigateUrl="~/DownloadFile/UserMannualAssosiate.html" Text="User Manual" target="popup" runat="server" CssClass="submitbutton" Width="130"  ></asp:HyperLink>--%>

                                    <asp:HyperLink Text="User Manual"

                                        runat="server" CssClass="submitbutton" Width="130" Style="margin-top:-7px" onClick="window.open('DownloadFile/UserMannualAssosiate.html','popup','toolbar=yes, scrollbars=yes, resizable=yes, top=500, left=50, width=800, height=900');return false"></asp:HyperLink>

                                    <%--<asp:Button ID="btnUserManual" Text="User Manual" runat="server" OnClick="UserManual" CssClass="submitbutton" Width="130" />--%>&nbsp;&nbsp;&nbsp;

                                </td>

                                <td>

                                    <asp:Button ID="btnExportExcel" Text="Export to Excel" OnClick="btnExportExcel_Click"

                                        runat="server" CssClass="submitbutton" Width="130" />

                                </td>

 

                                <%--<td>&nbsp;&nbsp;&nbsp;

                                    <asp:Button ID="btnAddNewFirm" Text="Add New Firm" OnClick="btnAddNewFirm_Click"

                                        runat="server" CssClass="submitbutton" Width="130" />

                                </td>--%>

                            </tr>

                        </table>

                    </td>

                </tr>

           </table>

            <div class="form-group">

                <asp:Panel ID="pnlAddBroker" runat="server" Width="100%" BorderColor="Silver" HorizontalAlign="Center"

                    Visible="false">

 

                 <%--   <table>

                        <tr>

                            <td></td>

                            <td></td>

                            <td></td>

                            <td></td>

                        </tr>

                         <tr>

                            <td></td>

                            <td></td>

                            <td></td>

                            <td></td>

                        </tr>

                    </table>--%>

 

 

                    <table class="table" style="border:1px solid #ddd" >

                        <tr>

                             <td style="width:100px;text-align:left;">Firm Name

                            </td>

                            <td style="text-align:left;">

                                <asp:Label runat="server" ID="lblBName" CssClass="textbox" Width="280px"></asp:Label>

                                <asp:TextBox ID="txtBrokerName" CssClass="textbox" Width="280px" runat="server" ValidationGroup="BMaster"></asp:TextBox>

                                <cc1:AutoCompleteExtender ServiceMethod="BrokerSearch" ServicePath="./WebService/AutoComplete.asmx"

                                    MinimumPrefixLength="1" CompletionInterval="100" EnableCaching="true" CompletionSetCount="10"

                                    OnClientItemSelected="ClientItemSelected" TargetControlID="txtBrokerName" ID="AutoCompleteExtender1"

                                    runat="server">

                                </cc1:AutoCompleteExtender>

                                <asp:RequiredFieldValidator ID="rfvBrokerName" ErrorMessage="*" CssClass="ErrorMessage"

                                    ControlToValidate="txtBrokerName" InitialValue="" runat="server" ValidationGroup="BMaster" />

                            </td>

                            <td></td>

                        </tr>

                        <tr>

                          

                           <%-- <td></td>--%>

                            <td style="width:150px;text-align:left; border-top:none; height:60px">Employee Type

                            </td>

                            <td style="text-align:left; border-top:none;">

                                <asp:DropDownList ID="ddlBrokerTypeMaster" CssClass="dropdowncss" runat="server"

                                    OnTextChanged="ddlBrokerTypeMaster_TextChanged" AutoPostBack="true" ValidationGroup="BMaster">

                               </asp:DropDownList>

                                <asp:RequiredFieldValidator ID="rfvBrokerTyp" ErrorMessage="*" CssClass="ErrorMessage"

                                    ControlToValidate="ddlBrokerTypeMaster" InitialValue="" runat="server" ValidationGroup="BMaster"></asp:RequiredFieldValidator>

                            </td>

                           <%--  <td></td> --%>

                            <td id="tdSectorName" runat="server" style="display: none;width:150px;text-align:left; border-top:none;">Sector Name

                            </td>

                            <td id="tdSectorValue" runat="server" style="display: none; position:absolute;border-top:none;">

                                <asp:DropDownList ID="ddlSectorName" CssClass="dropdowncss" runat="server"

                                    Width="200px" style="margin:-37px 149px">

                                </asp:DropDownList>

                                <asp:LinkButton ID="lnkAddSectorBtn" runat="server" Style="position:absolute;right:109px;top:-25px" OnClick="AddSector_Click" ValidationGroup="SMaster" Visible="true">Add</asp:LinkButton>

                                <asp:LinkButton ID="lnkEditBtn" runat="server" Style="position:absolute;right:68px;top:-25px" OnClick="lnkEditBtn_Click" ValidationGroup="SMaster" Visible="true">Edit</asp:LinkButton>

                                <asp:RequiredFieldValidator ID="rfvSectorName" ErrorMessage="*" CssClass="ErrorMessage"

                                    ControlToValidate="ddlSectorName" InitialValue="" runat="server" ValidationGroup="BMaster" />

                            </td>

                            <td id="tdaddSector" runat="server" style="display: none; border-top:none; position:relative">

                                <asp:TextBox ID="txtSectorName" CssClass="TextBoxcont" MaxLength="30" ValidationGroup="SMaster" Style="position:absolute;right:-157px;top:0px"

                                    Width="150px" runat="server" onkeypress="return AllowAlphabet(event)" xmlns:asp="#unknown"></asp:TextBox>

                                 <%----%>

                                <asp:LinkButton ID="lnkAddBtn" runat="server" Visible="false" OnClick="lnkAddBtn_Click" ValidationGroup="SMaster" Style="position:absolute;right:-195px;top:-1px">Add</asp:LinkButton>      

                                <asp:LinkButton ID="lnkUpdateBtn" runat="server" Visible="false" OnClick="lnkUpdateBtn_Click" ValidationGroup="SMaster" Style="position:absolute;right:-283px;top:-58px">Update</asp:LinkButton>

                                <asp:LinkButton ID="lnkDeleteBtn" runat="server" Visible="false" OnClick="lnkDeleteBtn_Click" ValidationGroup="SMaster" Style="position:absolute;right:-335px;top:-58px">Delete</asp:LinkButton>

                               <%-- <asp:RegularExpressionValidator ID="revNewSectorName" runat="server" ValidationGroup="SMaster" CssClass="ErrorMessage" InitialValue=""

                                    ControlToValidate="txtSectorName" ErrorMessage="Invaild input" ValidationExpression="[^0-9^+^\-^/^*^(^)]" Style="position:absolute;right:-146px;top:-41px"></asp:RegularExpressionValidator>                               --%>

 

                            </td>

                          

                        </tr>

                        <tr>

                            <td style="width:100px;text-align:left; border-top:none;">Contact name

                            </td>

                            <td style="text-align:left; border-top:none;">

                                <asp:TextBox ID="txtName" CssClass="textbox" runat="server" Width="150px" ValidationGroup="BMaster"></asp:TextBox>

                                <asp:RequiredFieldValidator ID="rfvName" ErrorMessage="*" CssClass="ErrorMessage"

                                   ControlToValidate="txtName" runat="server" InitialValue="" ValidationGroup="BMaster" />

                            </td>

                           <%-- <td></td>--%>

                            <td style="width:150px;text-align:left; border-top:none;">Mobile number

                            </td>

                            <td style="text-align:left; border-top:none;">

                                <asp:TextBox ID="txtNumber" CssClass="textbox" runat="server" MaxLength="10" ValidationGroup="BMaster"></asp:TextBox>

                                <cc1:FilteredTextBoxExtender runat="server" TargetControlID="txtNumber" FilterMode="ValidChars"

                                    FilterType="Numbers">

                                </cc1:FilteredTextBoxExtender>

                            </td>

                            <td style="width:150px;text-align:left; border-top:none;">Email ID

                            </td>

                            <td style="text-align:left; border-top:none;">

                                <asp:TextBox ID="txtEmail" CssClass="textbox" Width="200px" runat="server" ValidationGroup="BMaster"></asp:TextBox>

                                <asp:RegularExpressionValidator ID="rEvEmail" runat="server" ControlToValidate="txtEmail" InitialValue=""

                                    ErrorMessage="Invalid Email" CssClass="ErrorMessage" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"

                                    ValidationGroup="BMaster"></asp:RegularExpressionValidator>

                            </td>

                        </tr>

                        <%--  <tr>

                            <td>

                                <br />

                                <br />

                                SectorName Grid

                            </td>

                            <td>

                                <asp:panel runat="server" id="ss2" style="height: 125px; width: 100px;" xmlns:asp="#unknown">

                                <asp:gridview id="gvSectorName" runat="server" autogeneratecolumns="false" width="170px"

                                    showfooter="True" onrowcommand="gvMobile_RowCommand" gridlines="None" onrowdatabound="gvMobile_RowDataBound"

                                    tabindex="10">

                                    <columns>

                                    <asp:TemplateField HeaderText="Mobile" HeaderStyle-HorizontalAlign="Left">

                                        <ItemTemplate>

                                            <asp:Label ID="lblSectorName" runat="server" Text='<%# Eval("SectorName")%>'></asp:Label>

                                        </ItemTemplate>

                                        <HeaderTemplate>

                                        <asp:Label ID="rdSectorName" runat="server" Text="* Required" Visible="false" ForeColor="Red" > </asp:Label>

                                            <%--<asp:RegularExpressionValidator ID="revSectorName" runat="server"

                                                ControlToValidate="txtSectorNamegv" ErrorMessage="* valid no" ForeColor="Red" ValidationExpression="^[7-9]{1}.*[0-9]{9}$"></asp:RegularExpressionValidator>--%>

                        <%-- <asp:RegularExpressionValidator runat="server" ControlToValidate="txtSectorNamegv" ForeColor="Red" SetFocusOnError="true" Display="Dynamic"

                            ErrorMessage=" Restrict for special characters" ID="revSectorName" ValidationExpression="^[\sa-zA-Z0-9]*$"></asp:RegularExpressionValidator>

                        <asp:TextBox ID="txtSectorNamegv" HeaderStyle-HorizontalAlign="Left" CssClass="TextBoxcont" MaxLength="12"

                            Width="150px" runat="server" CausesValidation="true" OnTextChanged="ValuesTextBox_TextChanged"></asp:TextBox>

                        </HeaderTemplate>

                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">

                                        <%--<ItemTemplate>

                                            <asp:LinkButton ID="btnDelete" runat="server" Text="X" CommandName="SectorNameDelete"

                                                CommandArgument='<%# Container.DataItemIndex%>' OnClientClick="return confirm('Are you sure you want to delete this value?');"

                                                AlternateText="Delete"></asp:LinkButton>

                                        </ItemTemplate>--%>

                        <%--<headertemplate>

                                            <asp:LinkButton ID="btnAddSectorName" runat="server" Text="+" CommandName="SectorNameAdd"></asp:LinkButton>

                                        </headertemplate>

                                    </asp:TemplateField>

                    </columns>

                                </asp:gridview>

                </asp:Panel>

                </td>

                        </tr>--%>

                    </table>

                    <div class="row" style="padding-left: 180px">

                        <table class="tableAction">

                            <tr>

                                <td align="center">

                                    <asp:Button Text="Save" ID="btnSave" runat="server" ValidationGroup="BMaster" OnClick="btnSaveBrokerMaster"

                                        CssClass="submitbutton" />

                                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" OnClick="CancelBrokerGrading"

                                        CssClass="submitbutton" />

                                </td>

                            </tr>

                        </table>

                    </div>

                </asp:Panel>

                <asp:Panel runat="server" ID="UploadPannel" Visible="false">

                    <table>

                        <tr>

                            <td>

                                <asp:FileUpload ID="uploadexcel" runat="server" />

                            </td>

                            <td>

                                <asp:Button ID="btnbrokerupload" runat="server" Text="Upload" CssClass="submitbutton"

                                    OnClick="upload" />

                            </td>

                        </tr>

                    </table>

                </asp:Panel>

 

            </div>

            <%--<asp:Panel ID="PNLBrokerMater" runat="server" BorderColor="Silver" HorizontalAlign="Center">

        <fieldset id="Fieldset2" runat="server" style="background-position: center; font-family: 'Times New Roman'" class="header">BROKER MASTER  DETAILS</fieldset>

    </asp:Panel>--%>

            <asp:Label ID="lblCount" runat="server" Text="Count=" Font-Bold="true"></asp:Label>

            <asp:Label ID="lblrowcount" runat="server" Font-Bold="true"></asp:Label>

            <br />

            <asp:GridView ID="gvBrokerMaster" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found."

                AllowPaging="True" AlternatingRowStyle-CssClass="alt" PageSize="20" Width="100%"

                ShowHeaderWhenEmpty="True" CssClass="mGrid" OnPageIndexChanging="BrokerPageIndexChanging"

                EmptyDataRowStyle-HorizontalAlign="Center">

                <PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PageButtonCount="2"

                    PreviousPageText="Previous" />

                <Columns>

                    <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />

                    <asp:BoundField HeaderText="Dofa Code" DataField="DofaCode" />

                    <asp:BoundField HeaderText="Sector Name" DataField="SectorName" HtmlEncode="true" />

                    <asp:BoundField HeaderText="Broker Employee" DataField="BrokerType" HtmlEncode="false" />

                    <asp:BoundField HeaderText="Contact Name" DataField="Name" />

                    <asp:BoundField HeaderText="Contact Number" DataField="Number" />

                    <asp:BoundField HeaderText="Email ID" DataField="EmailID" />

                    <asp:TemplateField HeaderText="Max Limit" ItemStyle-HorizontalAlign="Center">

                        <ItemTemplate>

                            <asp:Label ID="lblMaxLimit" Text='<%# Eval("MaxLimit") %>' runat="server" />

                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">

                        <ItemTemplate>

                            <asp:HiddenField runat="server" ID="hfBrokerNameIDEdit" Value='<%#Eval("BrokerNameID") %>' />

                            <asp:Button ID="btnEdit" runat="server" CssClass="submitbutton" Height="30px" Width="50px"

                                Text="Edit" OnClick="EditBroker" CommandArgument="<%#((GridViewRow)Container).RowIndex %>"

                                CommandName='<%#Eval("BrokerID") %>' />

                            <asp:Button ID="btnDelete" runat="server" CssClass="submitbutton" Height="30px" Width="50px"

                                Text="Delete" CommandArgument='<%#Eval("BrokerID") %>' OnClientClick='return confirm("Do you want to delete this broker ?");'

                                OnClick="DeleteBrokerMaster" />

                        </ItemTemplate>

                    </asp:TemplateField>

                </Columns>

                <PagerStyle ForeColor="#FF3E3E" />

                <AlternatingRowStyle CssClass="alt" />

            </asp:GridView>

            <asp:GridView ID="gvExcel" runat="server" AutoGenerateColumns="false" EmptyDataText="No Row Found."

                AlternatingRowStyle-CssClass="alt" Width="100%" ShowHeaderWhenEmpty="True" CssClass="mGrid"

                EmptyDataRowStyle-HorizontalAlign="Center" Visible="false">

                <%--<PagerSettings Mode="NextPreviousFirstLast" NextPageText="Next" PageButtonCount="2" PreviousPageText="Previous" />--%>

                <Columns>

                    <asp:BoundField HeaderText="Broker Name" DataField="BrokerName" />

                    <asp:BoundField HeaderText="Sector Name" DataField="SectorName" HtmlEncode="true" />

                    <asp:BoundField HeaderText="Broker Employee" DataField="BrokerType" HtmlEncode="false" />

                    <asp:BoundField HeaderText="Contact Name" DataField="Name" />

                    <asp:BoundField HeaderText="Contact Number" DataField="Number" />

                    <asp:BoundField HeaderText="Email ID" DataField="EmailID" />

                    <asp:TemplateField HeaderText="Max Limit" ItemStyle-HorizontalAlign="Center">

                        <ItemTemplate>

                            <asp:Label ID="lblMaxLimit" Text='<%# Eval("MaxLimit") %>' runat="server" />

                            <asp:HiddenField runat="server" ID="hfBrokerNameIDEdit" Value='<%#Eval("BrokerNameID") %>' />

                        </ItemTemplate>

                    </asp:TemplateField>

                    <%--<asp:TemplateField HeaderText="Action" ItemStyle-HorizontalAlign="Center">

                        <ItemTemplate>

 

                            <asp:Button ID="btnEdit" runat="server" CssClass="submitbutton" Height="30px" Width="50px" Text="Edit" OnClick="EditBroker" CommandArgument="<%#((GridViewRow)Container).RowIndex %>" CommandName='<%#Eval("BrokerID") %>' />

                            <asp:Button ID="btnDelete" runat="server" CssClass="submitbutton" Height="30px" Width="50px" Text="Delete" CommandArgument='<%#Eval("BrokerID") %>' OnClientClick='return confirm("Do you want to delete this broker ?");' OnClick="DeleteBrokerMaster" />

                        </ItemTemplate>

                    </asp:TemplateField>--%>

                </Columns>

                <PagerStyle ForeColor="#FF3E3E" />

                <AlternatingRowStyle CssClass="alt" />

           </asp:GridView>

        </ContentTemplate>

        <Triggers>

            <asp:PostBackTrigger ControlID="btnbrokerupload" />

            <asp:PostBackTrigger ControlID="btnUpload" />

            <asp:PostBackTrigger ControlID="btnDownload" />

            <%--<asp:PostBackTrigger ControlID="btnUserManual" />--%>

            <asp:PostBackTrigger ControlID="btnExportExcel" />

        </Triggers>

    </asp:UpdatePanel>

    <script type="text/javascript">

        $("[id*=ddlBrokerTypeMaster]").change(function () {

            var value = $(this).val();

            var requiredValiator = $("[id*=rfvSectorName]");

            if (value == 1) {

                $("[id*=tdSectorName]").show();

                $("[id*=tdSectorValue]").show();

                // ValidatorEnable(requiredValiator[0], true);

            }

            else {

                $("[id*=tdSectorName]").hide();

                $("[id*=tdSectorValue]").hide();

                //ValidatorEnable(requiredValiator[0], false);

            }

        });





        function AllowAlphabet(e) {

            debugger;

            isIE = document.all ? 1 : 0

            keyEntry = !isIE ? e.which : event.keyCode;

            if (((keyEntry >= '65') && (keyEntry <= '90')) || ((keyEntry >= '97') && (keyEntry <= '122')) || (keyEntry == '46') || (keyEntry == '32') || keyEntry == '45')

                return true;

            else {

                alert('Please Enter Only Character values.');

                return false;

            }

        }

    </script>

</asp:Content>