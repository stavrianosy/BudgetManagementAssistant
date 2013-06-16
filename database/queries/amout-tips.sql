select sum(tipamount) as 'tips' from [Transaction]
where 
TransactionType_TypeTransactionId = 2 AND
TransactionDate > '05/25/2013'

select sum(amount) as 'amount w/ tips', sum(amount+tipamount) as 'Total'  from [Transaction]
where 
TransactionType_TypeTransactionId = 2 AND TipAmount > 0 AND
TransactionDate > '05/25/2013'