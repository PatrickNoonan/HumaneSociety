--SET IDENTITY_INSERT Categories OFF
--INSERT INTO Categories (CategoryId, Name)
--VALUES ('01', 'Cat')

--INSERT INTO Categories (CategoryId, Name)
--VALUES ('02', 'Dog')

--INSERT INTO Categories (CategoryId, Name)
--VALUES ('03', 'Bird')

--INSERT INTO Categories (CategoryId, Name)
--VALUES ('04', 'Micro Pig')

--INSERT INTO Categories (CategoryId, Name)
--VALUES ('05', 'Rabbit')

--UPDATE Categories
--SET 
--    Name = 'Rabbit'
--WHERE 
--	CategoryId = '05'
	--'02' = 'Dog',
	--'03' = 'Bird',
	--'04' = 'Micro Pig',
	--'05' = 'Rabbit'

--SET IDENTITY_INSERT DietPlans ON
--UPDATE DietPlans
--SET 
--	FoodType = 'KibblesNBits'
--WHERE 
--	DietPlanId = 1;
--UPDATE DietPlans
--SET 
--	FoodType = 'Leafy Greenz'
--WHERE 
--	DietPlanId = 5;

	--'02' = 'Dog',
	--'03' = 'Bird',
	--'04' = 'Micro Pig',
	--'05' = 'Rabbit'
--INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
--VALUES ('01', 'Kibble', 'kibbles', '3')

--INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
--VALUES ('02', 'Bit', 'bits', '5')

--INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
--VALUES ('03', 'Soft', 'Alpo', '2')

--INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
--VALUES ('04', 'Treats', 'Beggin strips', '1')

--INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
--VALUES ('05', 'Other', 'Catnip', '3')

INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
VALUES ('01', 'Kibble', 'kibbles', '3')

INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
VALUES ('02', 'Bit', 'bits', '5')

INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
VALUES ('03', 'Soft', 'Alpo', '2')

INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
VALUES ('04', 'Treats', 'Beggin strips', '1')

INSERT INTO DietPlans (DietPlanId, Name, FoodType, FoodAmountInCups)
VALUES ('05', 'Other', 'Catnip', '3')