mergeInto(LibraryManager.library, {

  AddCurScore: function(catScore,dogScore){
    document.getElementById('catScoreCur').innerHTML =parseInt(catScore);
    document.getElementById('dogScoreCur').innerHTML =parseInt(dogScore);
  },
  
  AddCurScore2: function(AvengersScore,ThanosScore){
    document.getElementById('AvengersScoreCur').innerHTML =parseInt(AvengersScore);
    document.getElementById('ThanosScoreCur').innerHTML =parseInt(ThanosScore);
  },
  AddRecord: function(win,tie,lose){
    var curWin = document.getElementById('win').innerHTML;
    var curTie = document.getElementById('tie').innerHTML;
    var curLose = document.getElementById('lose').innerHTML;
    document.getElementById('win').innerHTML =parseInt(win)+ parseInt(curWin);
    document.getElementById('tie').innerHTML =parseInt(tie)+ parseInt(curTie);
    document.getElementById('lose').innerHTML =parseInt(lose)+ parseInt(curLose);
  },

});
