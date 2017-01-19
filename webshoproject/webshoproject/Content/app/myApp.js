
(function () {
	'use strict';

	var myApp = angular.module('myApp', [
             
	]);

    
	myApp.controller('myCtrl', function ($scope, $http,$location) {
	      
			$http.get("/Cars/Getdatalist")
			   .then(function (response) {
			    $scope.carlist = response.data;
			      });
	    cash: false;
		    $scope.details = function (x) {
			 $scope.hidelist = true;
			 $scope.selectedcar = $scope.carlist[x];
			 $scope.selectedid = $scope.selectedcar.id;
			};
			$scope.gotolist= function () {
			    location.replace("http://localhost:54481/Cars/UserIndex");
		
			};
			$scope.Addtocart = function () {
			    var parameters = {
			        selectedcar: $scope.selectedid
			    };
			    var config = {
			        params: parameters
			    };
			    $http.get("/Cars/addtocart",config)
                  .then(function (response) {
                      $scope.feedback = response.data;
                      if ($scope.feedback == false) {
                          location.replace("http://localhost:54481/Account/Login");
                      };
                      if ($scope.feedback == true) {
                          $scope.hidelist = false;
                        
                      };
                  });

			};
	});


	})();