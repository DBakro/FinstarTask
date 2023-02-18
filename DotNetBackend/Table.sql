-- DROP TABLE public.codes;

CREATE TABLE public.codes (
	id int4 NOT NULL,
	code int4 NOT NULL,
	value text NOT NULL,
	CONSTRAINT codes_pk PRIMARY KEY (id)
);
CREATE INDEX codes_code_idx ON public.codes USING btree (code);
CREATE INDEX codes_value_idx ON public.codes USING btree (value);