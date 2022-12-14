import React from 'react';
import { cx } from '@emotion/css';
import { useForm, Controller } from 'react-hook-form';
import TextField from '@material-ui/core/TextField';
import Button from '@material-ui/core/Button';
import { Book } from './book.vm';
import * as classes from './book.styles';

interface Props {
  book: Book;
  onSave: (book: Book) => void;
  className?: string;
}

export const BookComponent: React.FunctionComponent<Props> = (props) => {
  const { className, book, onSave } = props;
  const { handleSubmit, control, reset } = useForm({
    defaultValues: book,
  });

  React.useEffect(() => {
    reset(book);
  }, [book]);

  return (
    <form
      className={cx(classes.root, className)}
      onSubmit={handleSubmit(onSave)}
    >
      <Controller
        name="title"
        control={control}
        render={({ field }) => (
          <TextField
            className={classes.title}
            inputProps={{ ...field }}
            label="Titulo"
            fullWidth={true}
          />
        )}
      />
      <Controller
        name="releaseDate"
        control={control}
        render={({ field }) => (
          <TextField
            className={classes.releaseDate}
            inputProps={{ ...field }}
            label="Fecha publicación"
            fullWidth={true}
            placeholder="dd/mm/aaaa"
            InputLabelProps={{ shrink: true }}
          />
        )}
      />
      <Controller
        name="author"
        control={control}
        render={({ field }) => (
          <TextField
            className={classes.author}
            inputProps={{ ...field }}
            label="Autor"
            fullWidth={true}
          />
        )}
      />
      <Button
        className={classes.submit}
        type="submit"
        variant="contained"
        color="primary"
      >
        Guardar
      </Button>
    </form>
  );
};
