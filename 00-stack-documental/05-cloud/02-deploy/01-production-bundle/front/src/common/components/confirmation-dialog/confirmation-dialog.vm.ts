export interface ConfirmationDialogLabelProps {
  closeButton: string;
  acceptButton?: string;
}

export const createEmptyConfirmationDialogLabelProps = (): ConfirmationDialogLabelProps => ({
  closeButton: '',
  acceptButton: '',
});
