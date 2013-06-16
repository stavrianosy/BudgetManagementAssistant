DECLARE @installemnt decimal(18,2)
DECLARE @exp decimal(18,2)
DECLARE @bills decimal(18,2)
DECLARE @inc decimal(18,2)
DECLARE @expStartDate datetime = '06/14/2013'--'05/25/2013'
DECLARE @incStartDate datetime = '05/20/2013'


select @installemnt = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and Category_CategoryId = 15 and 
TransactionDate > @expStartDate and IsDeleted = 0

select @bills = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and Category_CategoryId = 17 and 
TransactionDate > @expStartDate and IsDeleted = 0

select @exp = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and 
TransactionDate > @expStartDate and IsDeleted = 0

select @inc = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 1 and 
TransactionDate > @incStartDate and IsDeleted = 0


select nameofplace, amount, tipamount, TransactionDate, Comments from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and 
TransactionDate > @expStartDate and IsDeleted = 0


select @expStartDate as 'First exp', @installemnt as 'installments', @bills as 'bills', @exp as 'expences', @inc as 'income', @inc - @exp as 'balance'
select 'days left', datediff(day,GETDATE(), '06/30/2013')

