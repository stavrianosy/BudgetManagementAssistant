DECLARE @installemnt decimal(18,2)
DECLARE @exp decimal(18,2)
DECLARE @bills decimal(18,2)
DECLARE @inc decimal(18,2)


select @installemnt = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and Category_CategoryId = 15 and 
TransactionDate > '05/25/2013'

select @bills = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and Category_CategoryId = 17 and 
TransactionDate > '05/25/2013'

select @exp = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and 
TransactionDate > '05/25/2013'

select @inc = sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 1 and 
TransactionDate > '05/20/2013'


select 'installments', @installemnt
select 'bills', @bills
select 'expences', @exp
select 'income', @inc
select 'balance', @inc - @exp
select 'days left', datediff(day,GETDATE(), '06/30/2013')

