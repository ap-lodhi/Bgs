function verifyMaterials_Enquiry()
{
alert("BEFORE ")


 var Quantity=document.getElementById("ctl00_ContentPlaceHolder3_txtQuantity");
 if(Quantity.value == "")
    {
     alert("Select the item from the list");
     Quantity.focus();
     return false;
    }
 
//  var CategoryName=document.getElementById("ctl00_ContentPlaceHolder1_ddlCategoryName");
//   var SubCategory=document.getElementById("ctl00_ContentPlaceHolder1_ddlSubCategory");
//   var IdeaType=document.getElementById("ctl00_ContentPlaceHolder1_ddlIdeaType");
//    var briefOnIdea=document.getElementById("ctl00_ContentPlaceHolder1_txtBriefOnIdea");
//  var Investment=document.getElementById("ctl00_ContentPlaceHolder1_chkTermCond");
 
//    if(CategoryName.value == "--select--")
//    {
//     alert("Please select category");
//     CategoryName.focus();
//     return false;
//    }
//   if(SubCategory.value=="" )
//    {
//     alert("Please select SubCategory ");
//     SubCategory.focus();
//     return false;
//    } 

// else
//  if(SubCategory.value == "--select--")
//    {
//     alert("Please select SubCategory");
//     SubCategory.focus();
//     return false;
//    }
//    
//    
//    if(IdeaType.value == "--select--")
//    {
//     alert("Please select IdeaType");
//     IdeaType.focus();
//     return false;
//    }
//    
//    
//   if(briefOnIdea.value == "")
//    {
//     alert("Please enter brief idea");
//     briefOnIdea.focus();
//     return false;
//    }
//    if(!Investment.checked)
//{
//alert
//(
//"Please check the terms and conditions");
//return false; } 

}









//dispose: function() {


//AjaxControlToolkit.AutoCompleteBehavior.callBaseMethod(this, 'dispose');


//if (this._popupBehavior) {
//this._popupBehavior.dispose();
//this._popupBehavior = null;
//}
  //a Search User or Idea use web service AutoCompletExtenderAjax Control

function IAmSelected( source, eventArgs ) 
{  

 var ctrl=eventArgs.get_value();
 
  
  var Name=document.getElementById("ctl00_ddlSearch");
   var regExp = /\s+/g;
    if(Name.value == "User")
    {  
                           

      
      location="IdeaUser/" + ctrl.trim().replace(regExp,'-') ;
    }

     else if(Name.value == "Idea")
     {
     
     
       var myArr = new Array();
      myArr = ctrl.split("|..........|");

 for(var i=0;i<myArr.length;i++)
{
                     Idea=myArr[0];
                   User=myArr[1];
}
  
  location="IdeaUser/" + User.trim().replace(regExp,'-') +"/" +  Idea.trim().replace(regExp,'-') ;
    
    
     }

  
}

//  Minu of master page 

var timeout	= 500;
var closetimer	= 0;
var ddmenuitem	= 0;

// open hidden layer
function mopen(id)
{	
	// cancel close timer
	mcancelclosetime();

	// close old layer
	if(ddmenuitem) ddmenuitem.style.visibility = 'hidden';

	// get new layer and show it
	ddmenuitem = document.getElementById(id);
	ddmenuitem.style.visibility = 'visible';

}
// close showed layer
function mclose()
{
	if(ddmenuitem) ddmenuitem.style.visibility = 'hidden';
}

// go close timer
function mclosetime()
{
	closetimer = window.setTimeout(mclose, timeout);
}

// cancel close timer
function mcancelclosetime()
{
	if(closetimer)
	{
		window.clearTimeout(closetimer);
		closetimer = null;
	}
}

// close layer when click-out
document.onclick = mclose; 
// JScript File

function verify()
{
var Name=document.getElementById("ctl00_ContentPlaceHolder1_txtUserName");
 var Password=document.getElementById("ctl00_ContentPlaceHolder1_txtPassword");
 var ConfirmPassword=document.getElementById("ctl00_ContentPlaceHolder1_txtConfirmPassword");
 var FirstName=document.getElementById("ctl00_ContentPlaceHolder1_txtFirstName");
 var LastName=document.getElementById("ctl00_ContentPlaceHolder1_txtLastName");
 var AltEmail=document.getElementById("ctl00_ContentPlaceHolder1_txtEmail");
 var Catchp=document.getElementById("ctl00_ContentPlaceHolder1_txtCatchp");

    if(Name.value == "")
    {
     alert("Please enter username");
     Name.focus();
     return false;
    }

     else 
     {
           
         var sentence=Name.value;
             if (sentence.indexOf("@")!=-1 && EmailValidation(Name.value)==false )
             {
               alert("Please enter valid  email_Id like abc@gmail.com");
               Name.focus();
               return false;
              
              }
              }
    
     if(Password.value == "")
    {
     alert("Please enter password");
     Password.focus();
     return false;
    }
    
         if (Password.value .length  < 6)
     {
               alert("Password not Less  than 6 digits");
               Password.focus();
               return false;
     }
     
      if(ConfirmPassword.value == "")
    {
     alert("Please enter confirm password");
     ConfirmPassword.focus();
     return false;
    }
    if(ConfirmPassword.value !=Password.value)
    {
     alert("Confirm Password should  match with Password");
               ConfirmPassword.focus();
               return false;
    }
    
    
     if(FirstName.value == "")
    {
     alert("Please enter firstname");
     FirstName.focus();
     return false;
    }
     
    
     if(LastName.value == "")
    {
     alert("Please enter lastname ");
     LastName.focus();
     return false;
    }
       
      
     if (EmailValidation(AltEmail.value)==false )
             {
               alert("Please enter valid emailid abc@gmail.com");
               AltEmail.focus();
               return false;
              
              }
              
  
    
     if(Catchp.value == "")
    {
     alert("Please enter catchpa for varification");
     Catchp.focus();
     return false;
    }
    
    
    }
    function EmailValidation(string)
{ 
if(string.search(/^\w+((-\w+)|(\.\w+))*\@[A-Za-z0-9]+((\.|-)[A-Za-z0-9]+)*\.[A-Za-z0-9]+$/) != -1)
		return true;
	else
	return false;

}


function verifyUploadwish()
{



 var Title=document.getElementById("ctl00_ContentPlaceHolder1_txtWishtitle");
 
     var briefOnIdea=document.getElementById("ctl00_ContentPlaceHolder1_txtwish");

 



  
  if(Title.value == "")
    {
     alert("Please enter  title");
     IdeaTitle.focus();
     return false;
    }
  
    
    
  
    
   if(briefOnIdea.value == "")
    {
     alert("Please enter your wish");
     briefOnIdea.focus();
     return false;
    }
    
}





function verifyCategoryMaster()
{
var CategoryName=document.getElementById("ctl00_ContentPlaceHolder1_txtName");
 var Description=document.getElementById("ctl00_ContentPlaceHolder1_txtdescription");
  var Keywords=document.getElementById("ctl00_ContentPlaceHolder1_txtKeywords");

 
  
  if(CategoryName.value == "")
    {
     alert("Please enter CategoryName");
     CategoryName.focus();
     return false;
    }
      if(Description.value == "")
    {
     alert("Please enter Description");
     Description.focus();
     return false;
    }

 if(Keywords.value == "")
    {
     alert("Please enter Keywords");
     Keywords.focus();
     return false;
    }

}

function verifyUserProfiler()
{

var CountryId=document.getElementById("ctl00_ContentPlaceHolder1_ddlCountry");
 var StateId=document.getElementById("ctl00_ContentPlaceHolder1_ddlState");
  var CityName=document.getElementById("ctl00_ContentPlaceHolder1_ddlCity");
 
  


  if(CountryId.value == "select")
    {
     alert("Kindly select country name");
     CountryId.focus();
     return false;
    }
      if(StateId.value == "select")
    {
     alert("Kindly select state name");
     StateId.focus();
     return false;
    }

 if(CityName.value =="select")
    {
     alert("Kindly select city name");
     CityName.focus();
     return false;
    }
    
    // Radio Button Validation
// copyright Stephen Chapman, 15th Nov 2004,14th Sep 2005
// you may copy this function but please keep the copyright notice with it

   
          

}

           


function verifyStageMaster()
{

var StageName=document.getElementById("ctl00_ContentPlaceHolder1_txtStageName");
 var Description=document.getElementById("ctl00_ContentPlaceHolder1_txtdescription");
  
 
  
  if(CountryId.value == "select")
    {
     alert("Please enter StageName");
     StageName.focus();
     return false;
    }
      if(Description.value == "")
    {
     alert("Please enter Description");
     Description.focus();
     return false;
    }
}

function verifyCatApproverMapping()
{

var Category=document.getElementById("ctl00_ContentPlaceHolder1_ddlCategoryName");
var StageName=document.getElementById("ctl00_ContentPlaceHolder1_ddlStageName");
var StageNo=document.getElementById("ctl00_ContentPlaceHolder1_txtStageNo");

var Approver=document.getElementById("ctl00_ContentPlaceHolder1_lsbApprover");

   if(Category.value == "select")
    {    
     alert("Kindly select category name");
     Category.focus();
     return false;     
    }
    
    if(StageName.value == "select")
    {    
     alert("Kindly select stage name");
     StageName.focus();
     return false;     
    }
     if(StageNo.value < 1 )
    {
    alert(" Stage no should be grater then 0");
     StageNo.focus();
     return false;   
    
    }
    
if(Approver.value== "select")
{
  alert("Kindly select atleast one approver name");
     Approver.focus();
     return false;     
}


}

function verifyMyPage()
{

var ddlFilter=document.getElementById("ctl00_ContentPlaceHolder1_ddlFilter");
var txtsearch=document.getElementById("ctl00_ContentPlaceHolder1_txtsearch");
var ddlIdeas=document.getElementById("ctl00_ContentPlaceHolder1_ddlIdeas");


   if(ddlFilter.value == "")
    {    
     alert("Kindly select from dropdownlist");
     ddlFilter.focus();
     return false;     
    }
    
    if(txtsearch.value == "")
    {    
     alert("kindly enter search string");
     txtsearch.focus();
     return false;     
    }
   if(ddlIdeas.value == "")
    {    
     alert("Kindly select ideaTiitle from dropdownlist");
     ddlIdeas.focus();
     return false;     
    }
    


}




function SelectAll(id) {
        var frm = document.forms[0];
        for (i=0;i<frm.elements.length;i++) {
            if (frm.elements[i].type == "checkbox") {
                frm.elements[i].checked = document.getElementById(id).checked;
            }
        }
    } 


function verifyPostProblem()
{



 var Title=document.getElementById("ctl00_ContentPlaceHolder1_txtProbTitle");
 
     var Description=document.getElementById("ctl00_ContentPlaceHolder1_txtDesc");

 



  
  if(Title.value == "")
    {
     alert("Please enter problem title");
     Title.focus();
     return false;
    }
  
    
    
  
    
   if(Description.value == "")
    {
     alert("Please enter your Description");
     Description.focus();
     return false;
    }
    
}







function VarifyAddress()
{
var  PhonNo=document.getElementById("ctl00_ContentPlaceHolder1_txtphone");
var MobileNo=document.getElementById("ctl00_ContentPlaceHolder1_txtMobileNo");

//    start mobile & phone vailidation
 if(isNaN(PhonNo)||PhonNo.indexOf(" ")!=-1)
           {
              alert("Enter phone number numeric value")
              return false;
           }
           if (PhonNo.length>11)
           {
                alert("enter Phone Number 11 characters");
                return false;
           }
           if (PhonNo.charAt(0)!="0")
           {
                alert("Phone Number should start with 0 ");
                return false
           }

           if(isNaN(MobileNo)||MobileNo.indexOf(" ")!=-1)
           {
              alert("enter Mobile Number numeric value")
              return false;
           }
           if (MobileNo.length>12)
           {
                alert("enter mobile number 10 characters");
                return false;
           }
          
}



function VarifyProduct()
{
var  Title=document.getElementById("ctl00_ContentPlaceHolder1_txtTitle");
var  Description=document.getElementById("ctl00_ContentPlaceHolder1_txtdescription");
var  PricePerPiece=document.getElementById("ctl00_ContentPlaceHolder1_txtPricePerPiece");




     if(Title.value == "")
    {
     alert("Please enter Product title");
     Title.focus();
     return false;
    }

           
          if(Description.value == "")
    {
     alert("Please enter Product Description");
     Description.focus();
     return false;
    }
    
//     if(IsNumeric(PricePerPiece))
//           {
//              alert("PricePerPiece should be  numeric value")
//              return false;
//           }
          
}


function IsNumeric(sText)

{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
   
   }