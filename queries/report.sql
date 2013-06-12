select sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 2 and 
TransactionDate > '05/25/2013'

select sum(amount+tipamount) from [Transaction]
where 
TransactionType_TypeTransactionId = 1 and 
TransactionDate > '05/20/2013'