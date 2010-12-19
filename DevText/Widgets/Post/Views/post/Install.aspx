<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Install
    
    <%= DevText.Framework.Script.ScriptManager.RequiresScript("jQuery")%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Install</h2>
    
	<% using (Html.BeginForm())
    {%>
    <p>Click the button to intall this widget</p>
    <p>              
				<input type="submit" id="install" value="install" />
			</p>
    <%} %>

<script type="text/javascript">
    $(document).ready(function () {
        $("#install").click(function () {
           $.post("/Comment/Approve/", { check: "1" }, function () {
           alert("Widget installed");
           };
         });
    });
</script>

</asp:Content>
