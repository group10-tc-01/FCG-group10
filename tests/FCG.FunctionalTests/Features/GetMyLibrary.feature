Feature: Get My Library
As an authenticated user
I want to view my library
So that I can see all games I own

Scenario: Successfully retrieve my library with games
	Given I am an authenticated user
	And I have games in my library
	When I request to get my library
	Then the response status should be 200
	And the library should contain my games

Scenario: Retrieve empty library
	Given I am an authenticated user
	And I have no games in my library
	When I request to get my library
	Then the response status should be 200
	And the library should be empty

Scenario: Unauthorized access attempt
	Given I am not authenticated
	When I request to get my library
	Then the response status should be 401

Scenario: Admin user can view their library
	Given I am an authenticated admin user
	And I have games in my library
	When I request to get my library
	Then the response status should be 200
	And the library should contain my games
