<%@ Page language="C#" MasterPageFile="~masterurl/default.master" %> 

<asp:Content ID="Content1" ContentPlaceHolderId="PlaceHolderAdditionalPageHead" runat="server">

<style type="text/css">
    #MSO_ContentTable {padding:4px;background-color:white}
    #s4-leftpanel{display:none;}
    .s4-ca{margin-left:0px;}
    .myTable{padding:8px;border:solid black 1;}
    table.myTable td{padding:4px;}
</style>

<script type="text/javascript">
    function MyHandler(){
        alert('Hello ECMAScript');
    }
    function MyOtherHandler(){
        var span = document.getElementById('spanDisplayMessage');
        span.innerHTML = "Hello world!";
    }
	</script>
</asp:Content>
         
<asp:Content ContentPlaceHolderId="PlaceHolderMain" runat="server">
	<h2>Page 3</h2>    
  <hr/>
    <table class="myTable">
      <tr>
        <td valign="top">
            <input type="button" value="Run EcmaScript command1" onclick="MyHandler()" width="100px" />
        </td>
      </tr>
      <tr>
        <td valign="top">
            <input type="button" value="Run EcmaScript command2" onclick="MyOtherHandler()" width="100px" /><br/>
        </td>
      </tr>
      <tr>
        <td valign="top">
            <span style="font-size:18pt;"; id="spanDisplayMessage" />
        </td>
      </tr>
    </table>
</asp:Content>