<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/LoginMaster.master" AutoEventWireup="true" CodeBehind="ForgetPwd.aspx.cs" Inherits="BGS.ForgetPwd" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" language="javascript">


        function ResetPassword() {

            var txtLoginID = document.getElementById("ctl00_ContentPlaceHolder1_txtLoginID");
            if (txtLoginID.value == "") {
                alert("Enter LoginID");
                // document.getElementById("ctl00_ContentPlaceHolder1_txtUserID").focus();
                return false;


            }

            return true;

        }
        function isNumericKeyStroke() {
            var returnValue = false;
            var keyCode = (window.event.which) ? window.event.which : window.event.keyCode;

            if (((keyCode >= 48) && (keyCode <= 57)) || ((keyCode >= 96) && (keyCode <= 105)) || // All numerics
                       (keyCode == 8) ||     // Backspace
                       (keyCode == 13) // Carriage Return
                       || ((keyCode >= 48) && (keyCode <= 90)) //Alphabets
                       )

                returnValue = true;

            if (window.event.returnValue)
                window.event.returnValue = returnValue;

            return returnValue;
        }



        //  register for our events
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        function beginRequest(sender, args) {
            // show the popup
            $find('ctl00_ContentPlaceHolder1_mdlPopup').show();
        }

        function endRequest(sender, args) {
            //  hide the popup
            $find('ctl00_ContentPlaceHolder1_mdlPopup').hide();
        }

    </script>
    <div class="module"
        style="position: absolute; top: 156px; left: 20%; height: 145px; width: 500px">
        <div>
            <div style="border: 1px solid #dadada;">
                <div>
                    <br />
                    <center>
                        <h3>BGS Forgot Password</h3>
                    </center>


                    <center style="height: 78px">
                        &nbsp;
                        <asp:Label ID="Label1" runat="server" Text="Login ID" CssClass="LabelDetails"></asp:Label>
                        &nbsp;&nbsp;&nbsp; &nbsp; 
    <asp:TextBox ID="txtLoginID" runat="server" CssClass="TextBoxcont" MaxLength="10"></asp:TextBox>
                        <br />
                        <br />








                        <asp:Button ID="BtnReset" runat="server" Text="Reset Password" CssClass="submitbutton" Height="32px"
                            OnClick="BtnSubmit_Click" Width="114px" />



                        <br />



                    </center>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
