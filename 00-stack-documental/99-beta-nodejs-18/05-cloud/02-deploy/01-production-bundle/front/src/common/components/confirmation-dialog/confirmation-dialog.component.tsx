import React from 'react';
import Dialog from '@material-ui/core/Dialog';
import DialogTitle from '@material-ui/core/DialogTitle';
import DialogContent from '@material-ui/core/DialogContent';
import DialogActions from '@material-ui/core/DialogActions';
import Button from '@material-ui/core/Button';
import {
  createEmptyConfirmationDialogLabelProps,
  ConfirmationDialogLabelProps,
} from './confirmation-dialog.vm';

interface Props {
  isOpen: boolean;
  title: string | React.ReactNode;
  labels: ConfirmationDialogLabelProps;
  onClose: (event) => void;
  onAccept?: (event) => void;
  fullWidth?: boolean;
  className?: string;
}

export const ConfirmationDialogComponent: React.FunctionComponent<Props> = (
  props
) => {
  const {
    isOpen,
    title,
    labels,
    onAccept,
    onClose,
    fullWidth,
    className,
    children,
  } = props;

  const innerLabels = {
    ...createEmptyConfirmationDialogLabelProps(),
    ...labels,
  };
  const handleAccept = (event) => {
    onAccept(event);
    onClose(event);
  };

  return (
    <Dialog open={isOpen} className={className} fullWidth={fullWidth}>
      <DialogTitle>{title}</DialogTitle>
      <DialogContent>{children}</DialogContent>
      <DialogActions>
        <Button onClick={onClose} color="secondary" variant="contained">
          {innerLabels.closeButton}
        </Button>
        <Button onClick={handleAccept} color="primary" variant="contained">
          {innerLabels.acceptButton}
        </Button>
      </DialogActions>
    </Dialog>
  );
};

ConfirmationDialogComponent.defaultProps = {
  labels: createEmptyConfirmationDialogLabelProps(),
};
