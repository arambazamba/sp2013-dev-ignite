var officeDoc;


Office.initialize = function (reason) {

  officeDoc = Office.context.document;

  $(document).ready(function () {
    $("#cmdBindCell").click(onBindCell);
    $("#cmdBindTable").click(onBindTable);
  });
}


function onBindCell(event) {

  // set up binding options
  var bindingOptions = { id: "myFavoriateCell" };

  // bind to select cell
  officeDoc.bindings.addFromSelectionAsync("text",
                                           bindingOptions,
                                           onBindCellComplete);

}

function onBindCellComplete(asyncResult) {

  if (asyncResult.status == Office.AsyncResultStatus.Failed) {
    $("#display").text("ERROR: " + asyncResult.error.message);
    return;
  }


  // retrieve binding
  var binding = Office.select("bindings#myFavoriateCell");
  // add event handler
  binding.addHandlerAsync("bindingDataChanged",
                          onCellBindingDataChanged,
                          onCellBindingDataChanged);


}

function onCellBindingDataChanged(asyncResult) {

  // respond to data changing in cell
  var binding = Office.select("bindings#myFavoriateCell");
  binding.getDataAsync({ coercionType: "text" }, onNewCellDataRead);
}

function onNewCellDataRead(asyncResult) {

  var newText = asyncResult.value;
  $("#display").text("Cell Text: " + newText);

}


function onBindTable(event) {

  // create binding options
  var bindingOptions = {
    id: "dogTable"
  };

  // create binding to named table
  officeDoc.bindings.addFromNamedItemAsync("Sheet1!DogTable",
                                           "table",
                                           bindingOptions,
                                           onBindTableComplete);

}

function onBindTableComplete(asyncResult) {

  if (asyncResult.status == Office.AsyncResultStatus.Failed) {
    $("#display").text("ERROR: " + asyncResult.error.message);
    return;
  }

  // retrieve binding
  var binding = Office.select("bindings#dogTable");

  // wire up event handler
  binding.addHandlerAsync("bindingDataChanged",
                          onTableBindingDataChanged,
                          onTableBindingDataChanged);

}

function onTableBindingDataChanged(asyncResult) {

  if (asyncResult.status == Office.AsyncResultStatus.Failed) {
    $("#display").text("ERROR: " + asyncResult.error.message);
    return;
  }

  var binding = Office.select("bindings#dogTable");
  binding.getDataAsync({ coercionType: "table" }, onNewTableDataRead);

}

function onNewTableDataRead(asyncResult) {

  // empty out target div
  $("#display").empty();

  // create variable to reference table rows object
  var tableRows = asyncResult.value.rows;

  // create HTML table on the fly usin jQuery
  var table = $("<table>");

  var header = $("<tr>");
  header.append($("<th>").text("Dog"));
  header.append($("<th>").text("Breed"));
  table.append(header);

  // create row in HTML table for each row in Excel table
  for (var i = 0; i <= tableRows.length; i++) {
    if (tableRows[i]) {
      var tr = $("<tr>");
      tr.append($("<td>").text(tableRows[i][0]));
      tr.append($("<td>").text(tableRows[i][1]));
      table.append(tr);
    }
  }

  // add table to target div
  $("#display").append(table);
}
