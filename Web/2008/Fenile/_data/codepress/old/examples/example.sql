--
-- simple select example
-- 
SELECT * FROM books
	WHERE price > 100.00 and price < 150.00
	ORDER BY title

SELECT books.title, count(*) AS Authors
	FROM books
	JOIN book_authors 
		ON books.book_number = book_authors.book_number
	GROUP BY books.title

-- insert, update and delete examples
	
INSERT INTO my_table (field1, field2, field3) VALUES ('test', 'N', NULL);
UPDATE my_table SET field1 = 'updated value' WHERE field2 = 'N';
DELETE FROM my_table WHERE field2 = 'N';

BEGIN WORK;
	UPDATE inventory SET quantity = quantity - 3 WHERE item = 'pants';
COMMIT;